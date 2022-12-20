using System.ComponentModel.DataAnnotations;
using SantasWishlist.Domain;

namespace SantasWishList.Web.Models
{
    public class ChildWishListViewModel
    {
        public string SerializedChild { get; set; }
        public Dictionary<GiftCategory, List<string>> PossibleGifts { get; set; }
        
        [Required(ErrorMessage = "Je moet minimaal 1 cadeautje uit de lijst kiezen.")]
        public List<string> ChosenGiftNames { get; set; }
        
        [RegularExpression(@"^([A-Za-z0-9]+,\s*)*[A-Za-z0-9]+$", ErrorMessage = "Gebruik a-z, cijfers en comma's. Maar zeker dat er geen onnodige comma's of spaties overblijven.")]
        public string? AdditionalGiftNames { get; set; }
    }
}
