using SantasWishlist.Domain;

namespace SantasWishList.Web.Extensions;

public static class GiftCategoryExtension
{
    public static string ToFormattedText(this GiftCategory toReturn) => 
        toReturn switch
        {
            GiftCategory.NEED => "Dingen die je nodig hebt",
            GiftCategory.READ => "Dingen die je kan lezen",
            GiftCategory.WANT => "Dingen die je graag wil",
            GiftCategory.WEAR => "Dingen die je kan dragen"
        };
}