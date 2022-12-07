namespace SantasWishList.Web.Helpers;

public static class NavigationHelper
{
    public static string GetActiveMenuLinkCheck(string toCheck, string toCompare) => toCheck.ToLower() == toCompare.ToLower() ? "nav-link-active" : "";
}