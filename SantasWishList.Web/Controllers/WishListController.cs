using Microsoft.AspNetCore.Mvc;

namespace SantasWishList.Web.Controllers;

public class WishListController : Controller
{
    public WishListController()
    {
    }
    
    public IActionResult Index() => View(); 
}