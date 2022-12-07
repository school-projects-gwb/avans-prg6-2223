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
    
    public IActionResult Index() => View();

    public IActionResult CreateChildren(CreateChildrenViewModel model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");
        var names = ChildNameDataHelper.GetNamesFromData(model.NameData);
     
        //todo Create accounts
        
        throw new NotImplementedException();
    }
}