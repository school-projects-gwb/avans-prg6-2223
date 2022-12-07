using System.ComponentModel.DataAnnotations;

namespace SantasWishList.Web.Models;

public class CreateChildrenViewModel
{
    [Required]
    public string NameData { get; set; }
    
    [Required]
    public bool IsGood { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}