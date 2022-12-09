using SantasWishlist.Domain;

namespace SantasWishList.Web.Models;

public class ChildWishListConfirmViewModel
{
    public string SerializedChild { get; set; }
    public Dictionary<GiftCategory, List<string>> ChosenGifts { get; set; }
    public List<string> AdditionalGifts { get; set; }
}