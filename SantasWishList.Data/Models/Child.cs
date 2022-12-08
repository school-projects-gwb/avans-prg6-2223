using SantasWishlist.Domain;

namespace SantasWishList.Data.Models
{
    public class Child
    {
        public int Age { get; set; }
        public Behaviour Behaviour { get; set; }
        public string? Reasoning { get; set; }
        public WishList Wishlist { get; set; }
    }
    
    public enum Behaviour
    {
        BRAAF,
        BEETJE,
        STOUT
    }
}
