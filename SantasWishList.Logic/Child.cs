using System.ComponentModel.DataAnnotations;
using SantasWishlist.Domain;

namespace SantasWishList.Web.Logic
{
    public class Child
    {
        public string Name { get; set; }
        public bool IsNaughty { get; set; }
        public int Age { get; set; }
        public Behaviour Behaviour { get; set; }
        public string? Reasoning { get; set; }
        public WishList Wishlist { get; set; }
        public List<string>? AdditionalGiftNames { get; set; }
    }
    
    public enum Behaviour
    {
        [Display(Name = "Heel erg braaf!")]
        BRAAF,
        [Display(Name = "Een beetje braaf.")]
        BEETJE,
        [Display(Name = "Héél erg stout.")]
        STOUT
    }
}
