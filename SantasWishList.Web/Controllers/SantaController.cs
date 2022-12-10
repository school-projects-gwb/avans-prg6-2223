using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishList.Logic.Helpers;
using SantasWishList.Logic.Validation;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

[Authorize(Roles = "Santa")]
public class SantaController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly DuplicateUserValidator _duplicateUserValidator;
    
    public SantaController(UserManager<IdentityUser> userManager, DuplicateUserValidator duplicateUserValidator)
    {
        _userManager = userManager;
        _duplicateUserValidator = duplicateUserValidator;
    }

    [HttpGet]
    public IActionResult CreateChildren() => View();
    
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateChildren(CreateChildrenViewModel model)
    {
        //Validate and check for duplicate users
        if (!ModelState.IsValid) return View();
        var names = ChildNameDataHelper.GetNamesFromData(model.NameData);

        var validation = await _duplicateUserValidator.ValidateDuplicateUsers(names);

        if (validation != ValidationResult.Success)
        {
            ModelState.AddModelError("NameData", validation.ErrorMessage);
            return View();
        }

        //All data valid, create new users.
        foreach (string name in names)
        {
            IdentityUser user = new IdentityUser();
            user.UserName = name.ToLower();
            user.NormalizedUserName = name.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(user, new[]
                {
                    new Claim("IsNice", model.IsNice.ToString()),
                    new Claim("WishlistSubmitted", false.ToString())
                });
                
                await _userManager.AddToRoleAsync(user, "Child");
            }
        }

        //Make sure name data is nicely formatted and they have consistent spacing after each comma.
        model.NameData = ChildNameDataHelper.GetPrettyNameDataString(model.NameData);
        //todo decouple this view from post request
        return View("CreateChildrenSuccess", model);
    }
}