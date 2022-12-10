using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishList.Data.Models;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) => _signInManager = signInManager;
    
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        return View(new AccountViewModel
        {
            ReturnUrl = returnUrl
        });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AccountViewModel model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password.ToLower(), true, false);

            if (result.Succeeded) return LocalRedirect(model.ReturnUrl);

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout(string returnAction = "index")
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(returnAction, "Home");
    }
}