using System.ComponentModel.DataAnnotations;
using SantasWishList.Data.Models;
using SantasWishlist.Domain;

namespace SantasWishList.Web.Models
{
    public class ChildWishListViewModel
    {
        private List<Gift> PossibleGifts { get; set; }
        private Child Child { get; set; }
        [RegularExpression(@"^([A-Za-z0-9]+,\s*)*[A-Za-z0-9]+$", ErrorMessage = "Gebruik a-z, cijfers en comma's. Maar zeker dat er geen onnodige comma's of spaties overblijven.")]
        public string AdditionalGifts { get; set; }
    }
}
