using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using SantasWishList.Logic.Validation.AnnotationAttributes;

namespace SantasWishList.Web.Models;

public class CreateChildrenViewModel
{
    [Required]
    [ChildNameData]
    public string NameData { get; set; }
    
    [Required]  
    public bool IsGood { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(3, ErrorMessage = "Wachtwoord moet minimaal 3 letters lang zijn.")]
    [MaxLength(10, ErrorMessage = "Wachtwoord mag maximaal 10 letters lang zijn.")]
    [RegularExpression(@"^[a-z]+$", ErrorMessage = "Gebruik a-z, kleine letters.")]
    public string Password { get; set; }
}