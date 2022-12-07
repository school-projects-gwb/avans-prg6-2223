using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishList.Data.Models;
using SantasWishList.Logic.Helpers;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

[Authorize]
public class WishListController : Controller
{
    private readonly UserManager<User> _userManager;
    
    public WishListController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult CreateChildren() => View();

    [Authorize(Roles = "Admin")]
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
            User user = new User();
            user.UserName = name.ToLower();
            user.IsGood = model.IsGood;
            user.NormalizedUserName = name.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();
            
            await _userManager.CreateAsync(user, model.Password);
        }

        //Make sure name data is nicely formatted and they have consistent spacing between comma's.
        model.NameData = ChildNameDataHelper.GetPrettyNameDataString(model.NameData);
        
        return View("CreateChildrenSuccess", model);
    }
}