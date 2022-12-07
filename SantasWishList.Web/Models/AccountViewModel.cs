using System.ComponentModel.DataAnnotations;

namespace SantasWishList.Web.Models;

public class AccountViewModel
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    public string ReturnUrl { get; set; }
}