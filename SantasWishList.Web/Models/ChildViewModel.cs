using Microsoft.Build.Framework;
using SantasWishlist.Domain;
using System.Text.Json.Nodes;

namespace SantasWishList.Web.Models
{
    public class ChildViewModel
    {
        [Required]
        public int Age { get; set; }
        [Required]
        public Behaviour Behaviour { get; set; }
        [Required]
        public bool HasBeenNice { get; set; }
        [Required]
        public string Reasoning { get; set; }
        [Required]
        public Dictionary<GiftCategory, Gift> Wishlist { get; set; }
    }
    
    public enum Behaviour
    {
        BRAAF,
        BEETJE,
        STOUT
    }
}
