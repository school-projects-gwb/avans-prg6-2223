using Microsoft.Build.Framework;
using SantasWishList.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace SantasWishList.Web.Models;
using Microsoft.Build.Framework;
using System.Configuration;

public class ChildAboutViewModel
{
    [Required]
    [RegularExpression("([0-9]+)", ErrorMessage ="Voer alleen vijfers in")]
    [Range(typeof(int), "3", "18", ErrorMessage ="Je moet tussen de 3 en 18 jaar oud zijn.")]
    public int Age { get; set; }
    
    [Required]
    public Behaviour Behaviour { get; set; }

    [StringLength(500, MinimumLength = 50, ErrorMessage = "Je moet minimaal 50 tekens invoeren en maximaal 500")]
    public string? Reasoning { get; set; }
}