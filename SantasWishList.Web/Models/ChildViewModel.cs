using Microsoft.Build.Framework;
using System.Text.Json.Nodes;

namespace SantasWishList.Web.Models
{
    public class ChildViewModel
    {
        [Required]
        public int age { get; set; }
        [Required]
        public behaviour behaviour { get; set; }
        [Required]
        public string reasoning { get; set; }

    }
    
    public enum behaviour
    {
        Braaf,
        Beetje,
        Stout
    }
}
