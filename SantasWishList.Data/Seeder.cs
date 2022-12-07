using Microsoft.AspNetCore.Identity;
using SantasWishList.Data.Models;

namespace SantasWishList.Data;

public class Seeder
{
	private readonly int userId = 1;

	public IEnumerable<User> UserSeeder() => new List<User>()
	{
		new User
		{
			Id = userId,
			UserName = "santa",
			NormalizedUserName = "SANTA",
			PasswordHash = new PasswordHasher<User>().HashPassword(null, "wachtwoord"),
			SecurityStamp = Guid.NewGuid().ToString()
		}
	};

    public IEnumerable<Role> RoleSeeder() => new List<Role>()
    {
	    new Role
	    {
		    Id = 1,
		    Name = "Admin",
		    NormalizedName = "ADMIN"
	    },
	    new Role
	    {
		    Id = 2,
		    Name = "Temp",
		    NormalizedName = "TEMP"
	    }
    };

    public IEnumerable<IdentityUserRole<int>> IdentityUserRoleSeeder() => new List<IdentityUserRole<int>>()
    {
	    new IdentityUserRole<int>
	    {
		    UserId = userId,
		    RoleId = 1
	    }
    };
}