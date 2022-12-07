using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishList.Data.Models;
using SantasWishList.Logic.Helpers;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

public class WishListController : Controller
{
    private readonly UserManager<User> _userManager;
    
    public WishListController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public IActionResult CreateChildren() => View();

    [HttpPost]
    public async Task<IActionResult> CreateChildren(CreateChildrenViewModel model)
    {
        if (!ModelState.IsValid) return View();
        var names = ChildNameDataHelper.GetNamesFromData(model.NameData);
        
        foreach (string name in names)
        {
            User user = new User();
            user.UserName = name.ToLower();
            user.IsGood = model.IsGood;
            user.NormalizedUserName = name.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();
            
            // await _userManager.CreateAsync(user, model.Password);
        }

        //todo Create accounts
        
        throw new NotImplementedException();
    }
}