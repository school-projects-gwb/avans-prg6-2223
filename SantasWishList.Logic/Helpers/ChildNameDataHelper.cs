namespace SantasWishList.Logic.Helpers;

public static class ChildNameDataHelper
{
    public static List<string> GetNamesFromData(string nameData) => 
        nameData.Split(',')
        .Select(name => name.Trim())
        .Where(name => !string.IsNullOrWhiteSpace(name))
        .ToList();

    public static bool DataHasDuplicates(string nameData)
    {
        var check = GetNamesFromData(nameData);
        return check.Count() != check.Distinct().Count();
    }
}