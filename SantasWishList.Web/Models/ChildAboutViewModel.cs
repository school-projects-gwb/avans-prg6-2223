using Microsoft.Build.Framework;
using SantasWishList.Data.Models;

namespace SantasWishList.Web.Models;

public class ChildAboutViewModel
{
    [Required]
    public int Age { get; set; }
    
    [Required]
    public Behaviour Behaviour { get; set; }
    
    public string? Reasoning { get; set; }
}