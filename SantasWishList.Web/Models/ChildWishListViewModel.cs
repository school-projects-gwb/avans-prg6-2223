using System.ComponentModel.DataAnnotations;
using SantasWishList.Data.Models;
using SantasWishlist.Domain;

namespace SantasWishList.Web.Models
{
    public class ChildWishListViewModel
    {
        private List<Gift> PossibleGifts { get; set; }
        private Child Child { get; set; }
    }
}
