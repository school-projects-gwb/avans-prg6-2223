using SantasWishList.Data.Models;

namespace SantasWishList.Web.Extensions;

public static class BehaviourExtension
{
    public static string ToBehaviourText(this Behaviour toReturn) => 
        toReturn switch
        {
            Behaviour.BRAAF => "Super braaf!",
            Behaviour.BEETJE => "Een klein beetje braaf.",
            Behaviour.STOUT => "Héél erg stout"
        };
}