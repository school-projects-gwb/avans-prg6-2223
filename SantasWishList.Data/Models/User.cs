using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace SantasWishList.Data.Models
{
	public class User : IdentityUser<int>
    {
		public bool IsLocked { get; set; }
		
		public bool IsGood { get; set; }
	}
}

