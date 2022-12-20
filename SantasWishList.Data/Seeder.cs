using Microsoft.AspNetCore.Identity;

namespace SantasWishList.Data;

public class Seeder
{
	private const string UserId = "1";

	public IEnumerable<IdentityUser> UserSeeder() => new List<IdentityUser>()
	{
		new IdentityUser
		{
			Id = UserId,
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
		    UserId = UserId,
		    RoleId = "1"
	    }
    };
}