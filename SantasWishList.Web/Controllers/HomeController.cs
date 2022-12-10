using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SantasWishList.Data.Models;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        //Redirect to correct 'default' action based on role
        if (User.IsInRole("Santa")) return RedirectToAction("CreateChildren", "Santa");
        if (User.IsInRole("Child")) return RedirectToAction("ChildAbout", "WishList");
        //Fallback
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [HttpGet]
    public IActionResult WishListSuccess() => View();

    [HttpGet]
    [Route("WishListError")]
    public IActionResult WishListError() => View();
}

