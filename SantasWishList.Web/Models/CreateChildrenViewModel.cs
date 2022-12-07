using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SantasWishList.Web.Models;

public class CreateChildrenViewModel : IValidatableObject
{
    [Required]
    public string NameData { get; set; }
    
    [Required]  
    public bool IsGood { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(3, ErrorMessage = "Wachtwoord moet minimaal 3 letters lang zijn.")]
    [MaxLength(10, ErrorMessage = "Wachtwoord mag maximaal 10 letters lang zijn.")]
    [RegularExpression(@"^[a-z]+$", ErrorMessage = "Gebruik a-z, kleine letters.")]
    public string Password { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        var regex = new Regex(@"(/^(([a-zA-Z0-9 ](,)?)*)+$/)");

        if (!regex.IsMatch(NameData))
        {
            yield return new ValidationResult(
                errorMessage: "fout.",
                memberNames: new[] { "NameData" }
            );
        }
    }
}