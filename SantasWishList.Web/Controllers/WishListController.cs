using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SantasWishlist.Domain;
using SantasWishList.Logic;
using SantasWishList.Logic.Helpers;
using SantasWishList.Logic.Validation;
using SantasWishList.Web.Logic;
using SantasWishList.Web.Models;

namespace SantasWishList.Web.Controllers;

[Authorize(Roles = "Child")]
public class WishListController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ChildWishListBuilder _childWishListBuilder;
    private readonly IGiftRepository _giftRepository;
    private readonly WishListValidator _wishListValidator;
    
    public WishListController(UserManager<IdentityUser> userManager, ChildWishListBuilder childWishListBuilder,
        IGiftRepository giftRepository, WishListValidator wishListValidator)
    {
        _userManager = userManager;
        _childWishListBuilder = childWishListBuilder;
        _giftRepository = giftRepository;
        _wishListValidator = wishListValidator;
    }
    
    [HttpGet]
    public IActionResult ChildAbout()
    {
        ViewBag.Name = User.Identity.Name;
        return View();
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ChildAboutSubmit(ChildAboutViewModel model)
    {
        if (!ModelState.IsValid) return View("ChildAbout");
        
        TempData["SerializedChild"] = _childWishListBuilder
            .SetName(User.Identity.Name)
            .SetIsNice(Convert.ToBoolean(User.Claims.FirstOrDefault(claim => claim.Type.Equals("IsNice")).Value))
            .SetAge(model.Age)
            .SetBehaviour(model.Behaviour)
            .SetReasoning(model.Reasoning)
            .Serialize();

        return RedirectToAction("ChildWishList");
    }
    
    [HttpGet]
    public IActionResult ChildWishListRedirect(string serializedChild)
    {
        TempData["SerializedChild"] = serializedChild;
        return Redirect("ChildWishList");
    }
    
    [HttpGet]
    public IActionResult ChildWishList()
    {
        if (!TempData.ContainsKey("SerializedChild")) return RedirectToAction("ChildAbout");

        var viewModel = GetChildWishListViewModel();
        viewModel.SerializedChild = TempData["SerializedChild"].ToString();
        ViewBag.Name = User.Identity.Name;

        return View(viewModel);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ChildWishListSubmit(ChildWishListViewModel model)
    {
        //Don't validate PossibleGifts.
        ModelState.Remove("PossibleGifts");
        if (!ModelState.IsValid) return View("ChildWishList", GetChildWishListViewModel());
        
        //Deserialize and set Child wishlist and gift data
        Child child = _childWishListBuilder
            .Deserialize(model.SerializedChild)
            .SetWishList(_giftRepository.GetPossibleGifts().Where(gift => model.ChosenGiftNames.Contains(gift.Name)).ToList())
            .SetAdditionalGiftNames(model.AdditionalGiftNames == null ? null : ChildNameDataHelper.GetNamesFromData(model.AdditionalGiftNames))
            .Build();
        
        TempData["SerializedChild"] = _childWishListBuilder.Serialize();
        
        //Validate child, return errors when present
        List<ValidationResult> errorList = _wishListValidator.ValidateWishList(child);
        
        foreach(ValidationResult result in errorList.Where(vr => vr != ValidationResult.Success))
            ModelState.AddModelError("error", result.ErrorMessage);
        
        if (errorList.Any(vr => vr != ValidationResult.Success)) 
            return View("ChildWishList", GetChildWishListViewModel());

        return RedirectToAction("ChildWishListConfirm");
    }
    
    [HttpGet]
    public IActionResult ChildWishListConfirm()
    {
        if (!TempData.ContainsKey("SerializedChild")) return RedirectToAction("ChildAbout");
        
        var child = _childWishListBuilder.Deserialize(TempData["SerializedChild"].ToString()).Build();

        var viewModel = new ChildWishListConfirmViewModel();
        viewModel.SerializedChild = TempData["SerializedChild"].ToString();
        viewModel.ChosenGifts = GetGroupedGifts(child.Wishlist.Wanted);
        viewModel.AdditionalGifts = child.AdditionalGiftNames;

        return View(viewModel);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChildWishListConfirmSubmit(ChildWishListConfirmViewModel model)
    {
        Child child = _childWishListBuilder.Deserialize(model.SerializedChild).Build();

        //Validate child about once more
        var childAbout = new ChildAboutViewModel 
            { Age = child.Age, Behaviour = child.Behaviour, Reasoning = child.Reasoning};

        var aboutValidationContext = new ValidationContext(childAbout, serviceProvider: null, items: null);
        var aboutValidationResult = new List<ValidationResult>();

        if (!Validator.TryValidateObject(childAbout, aboutValidationContext, aboutValidationResult, true))
            return RedirectToAction("WishListError", "Home");
        
        //Validate child and send wishlist, return to error page when this goes wrong.
        bool isValid = !_wishListValidator.ValidateWishList(child).Where(vr => vr != ValidationResult.Success).Any(),
             isSent = _giftRepository.SendWishList(child.Wishlist);

        if (!isValid || !isSent)
            return RedirectToAction("WishListError", "Home");
        
        //Delete user
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        await _userManager.DeleteAsync(user);

        return RedirectToAction("LogOut", "Account", new { returnAction = "WishListSuccess" });
    }

    private Dictionary<GiftCategory, List<string>> GetGroupedGifts(List<Gift> gifts) => gifts
        .GroupBy(gift => gift.Category)
        .ToDictionary(gift => gift.Key, gift => gift.Select(gift => gift.Name)
        .ToList());
    
    private ChildWishListViewModel GetChildWishListViewModel()
    {
        ChildWishListViewModel viewModel = new ChildWishListViewModel();
        viewModel.PossibleGifts = GetGroupedGifts(_giftRepository.GetPossibleGifts());

        return viewModel;
    }
}