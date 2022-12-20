using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using SantasWishList.Logic.Helpers;

namespace SantasWishList.Logic.Validation.AnnotationAttributes;

public class ChildNameData : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        if (ChildNameDataHelper.DataHasDuplicates(value.ToString()))
            return new ValidationResult(context.DisplayName + " mag enkel unieke namen bevatten.");
        
        if (new Regex(@"^([a-zA-Z]+,\s*)*[a-zA-Z]+$").IsMatch(value.ToString())) return ValidationResult.Success;

        return new ValidationResult(context.DisplayName + " is niet valide.");
    }
}