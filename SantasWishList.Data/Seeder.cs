using Microsoft.AspNetCore.Identity;
using SantasWishList.Data.Models;

namespace SantasWishList.Data;

public class Seeder
{
	private readonly string userId = "1";

	public IEnumerable<IdentityUser> UserSeeder() => new List<IdentityUser>()
	{
		new IdentityUser
		{
			Id = userId,
			UserName = "santa",
			NormalizedUserName = "SANTA",
			PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "wachtwoord"),
			SecurityStamp = Guid.NewGuid().ToString()
		}
	};

    public IEnumerable<IdentityRole> RoleSeeder() => new List<IdentityRole>()
    {
	    new IdentityRole
	    {
		    Id = "1",
		    Name = "Santa",
		    NormalizedName = "SANTA"
	    },
	    new IdentityRole
	    {
		    Id = "2",
		    Name = "Child",
		    NormalizedName = "CHILD"
	    }
    };

    public IEnumerable<IdentityUserRole<string>> IdentityUserRoleSeeder() => new List<IdentityUserRole<string>>()
    {
	    new IdentityUserRole<string>
	    {
		    UserId = userId,
		    RoleId = "1"
	    }
    };
}