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
    [HttpGet]
    public IActionResult CreateChildren() => View();

    [Authorize(Roles = "Santa")]
    [HttpPost, ValidateAntiForgeryToken]
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

    [Authorize(Roles = "Child")]
    [HttpGet]
    public IActionResult ChildAbout()
    {
        ViewBag.Name = User.Identity.Name;
        return View();
    }

    [Authorize(Roles = "Child")]
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

    [Authorize(Roles = "Child")]
    [HttpGet]
    public IActionResult ChildWishList()
    {
        if (!TempData.ContainsKey("SerializedChild")) return RedirectToAction("ChildAbout");

        var viewModel = GetChildWishListViewModel();
        viewModel.SerializedChild = TempData["SerializedChild"].ToString();
        ViewBag.Name = User.Identity.Name;

        return View(viewModel);
    }
    
    [Authorize(Roles = "Child")]
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ChildWishListSubmit(ChildWishListViewModel model)
    {
        ModelState.Remove("PossibleGifts");
        if (!ModelState.IsValid) return View("ChildWishList", GetChildWishListViewModel());

        // List<ValidationResult> errorList = _wishListValidator.ValidateWishList(_childWishListBuilder.Deserialize(model.SerializedChild).Build());
        //
        // foreach(ValidationResult result in errorList)
        // {
        //     if(result != ValidationResult.Success)
        //     {
        //         ModelState.AddModelError("error", result.ErrorMessage);
        //         return View("ChildWishListConfirm");
        //     }
        // }

        TempData["SerializedChild"] = _childWishListBuilder
                .Deserialize(model.SerializedChild)
                .SetWishList(_giftRepository.GetPossibleGifts().Where(gift => model.ChosenGiftNames.Contains(gift.Name)).ToList())
                .SetAdditionalGiftNames(model.AdditionalGiftNames == null ? null : ChildNameDataHelper.GetNamesFromData(model.AdditionalGiftNames))
                .Serialize();

        return RedirectToAction("ChildWishListConfirm");
    }

    [Authorize(Roles = "Child")]
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

    [Authorize(Roles = "Child")]
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ChildWishListConfirmSubmit(ChildWishListConfirmViewModel model)
    {
        throw new NotImplementedException();
    }

    private Dictionary<GiftCategory, List<string>> GetGroupedGifts(List<Gift> gifts) => gifts
        .GroupBy(gift => gift.Category)
        .ToDictionary(gift => gift.Key, gift => gift.Select(gift => gift.Name).ToList());
    
    private ChildWishListViewModel GetChildWishListViewModel()
    {
        ChildWishListViewModel viewModel = new ChildWishListViewModel();
        viewModel.PossibleGifts = GetGroupedGifts(_giftRepository.GetPossibleGifts());

        return viewModel;
    }
}