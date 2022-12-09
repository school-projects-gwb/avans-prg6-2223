using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SantasWishList.Data.Models;
using SantasWishlist.Domain;
using SantasWishList.Logic;
using SantasWishList.Logic.Helpers;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

[Authorize]
public class WishListController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ChildWishListBuilder _childWishListBuilder;
    
    public WishListController(UserManager<IdentityUser> userManager, ChildWishListBuilder childWishListBuilder)
    {
        _userManager = userManager;
        _childWishListBuilder = childWishListBuilder;
    }

    [Authorize(Roles = "Santa")]
    public IActionResult CreateChildren() => View();

    [Authorize(Roles = "Santa")]
    [HttpPost]
    public async Task<IActionResult> CreateChildren(CreateChildrenViewModel model)
    {
        if (!ModelState.IsValid) return View();
        var names = ChildNameDataHelper.GetNamesFromData(model.NameData);
        
        //todo possibly move to somewhere in logic layer
        //Check duplicates and return with list of duplicate usernames
        var existingUsers = new List<string>();   
        foreach (var name in names)
            if (await _userManager.FindByNameAsync(name) != null) existingUsers.Add(name);
        
        if (existingUsers.Count > 0)
        {
            ModelState.AddModelError(
                "NameData",
        "De volgende kinderen zijn al geregistreerd: " + string.Join(", ", existingUsers.ToArray()));
            return View();
        }

        //All data valid, create new users.
        foreach (string name in names)
        {
            IdentityUser user = new IdentityUser();
            user.UserName = name.ToLower();
            user.NormalizedUserName = name.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();
            
            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddClaimsAsync(user, new[]
            {
                new Claim("IsNice", model.IsNice.ToString()),
                new Claim("WishlistSubmitted", false.ToString())
            });
            await _userManager.AddToRoleAsync(user, "Child");
        }

        //Make sure name data is nicely formatted and they have consistent spacing after each comma.
        model.NameData = ChildNameDataHelper.GetPrettyNameDataString(model.NameData);
        
        return View("CreateChildrenSuccess", model);
    }

    [Authorize(Roles = "Child")]
    public IActionResult ChildAbout()
    {
        ViewBag.Name = User.Identity.Name;
        return View();
    }

    [Authorize(Roles = "Child")]
    [HttpPost]
    public IActionResult ChildAboutSubmit(ChildAboutViewModel model)
    {
        if (!ModelState.IsValid) return View("ChildAbout");
        
        ChildWishListViewModel viewModel = new ChildWishListViewModel();
        
        viewModel.SerializedChild = _childWishListBuilder
            .SetName(User.Identity.Name)
            .SetIsNice(Convert.ToBoolean(User.Claims.FirstOrDefault(claim => claim.Type.Equals("IsNice")).Value))
            .SetAge(model.Age)
            .SetBehaviour(model.Behaviour)
            .SetReasoning(model.Reasoning)
            .Serialize();
        
        return RedirectToAction("ChildWishList", viewModel);
    }

    public IActionResult ChildWishList(ChildWishListViewModel model)
    {
        return View(model);
    }

    [HttpPost]
    public IActionResult ChildWishListSubmit(ChildWishListViewModel model)
    {
        model.SerializedChild =
            _childWishListBuilder
                .Deserialize(model.SerializedChild)
                .SetWishList(model.ChosenGifts)
                .SetAdditionalGiftNames(ChildNameDataHelper.GetNamesFromData(model.AdditionalGifts))
                .Serialize();
        
        return View("ChildWishListConfirm", model);
    }

    public IActionResult ChildWishListConfirm(ChildWishListViewModel model)
    {
        return View(model);
    }
}