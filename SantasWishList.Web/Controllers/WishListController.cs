using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishlist.Domain;
using SantasWishList.Logic;
using SantasWishList.Logic.Helpers;
using SantasWishList.Logic.Validation;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

[Authorize]
public class WishListController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ChildWishListBuilder _childWishListBuilder;
    private readonly IGiftRepository _giftRepository;
    private readonly WishListValidation _wishListValidator;
    
    public WishListController(UserManager<IdentityUser> userManager, ChildWishListBuilder childWishListBuilder,
        IGiftRepository giftRepository, WishListValidation wishListValidator)
    {
        _userManager = userManager;
        _childWishListBuilder = childWishListBuilder;
        _giftRepository = giftRepository;
        _wishListValidator = wishListValidator;
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
        //Build viewmodel
        ChildWishListViewModel viewModel = new ChildWishListViewModel();
        viewModel.PossibleGifts = 
            _giftRepository
                .GetPossibleGifts().GroupBy(gift => gift.Category)
                .ToDictionary(gift => gift.Key, gift => gift.Select(gift => gift.Name).ToList());
        
        //Add "about" data to child
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
        ViewBag.Name = User.Identity.Name;
        return View(model);
    }

    [HttpPost]
    public IActionResult ChildWishListSubmit(ChildWishListViewModel model)
    {
        if (!ModelState.IsValid) return View("ChildWishList");
        //Add gift data to child
        model.SerializedChild =
            _childWishListBuilder
                .Deserialize(model.SerializedChild)
                .SetWishList(model.ChosenGifts)
                .SetAdditionalGiftNames(ChildNameDataHelper.GetNamesFromData(model.AdditionalGiftNames))
                .Serialize();
        
        List<ValidationResult> errorList = _wishListValidator.ValidateWishList(_childWishListBuilder.Deserialize(model.SerializedChild).Build());

        foreach(ValidationResult result in errorList)
        {
            if(result != ValidationResult.Success)
            {
                ModelState.AddModelError("error", result.ErrorMessage);
                return View("ChildWishListConfirm");
            }
        }

        return View("ChildWishListConfirm", model);
    }

    public IActionResult ChildWishListConfirm(ChildWishListViewModel model)
    {
        return View(model);
    }
}