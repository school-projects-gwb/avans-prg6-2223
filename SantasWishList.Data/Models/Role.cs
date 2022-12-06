using Microsoft.AspNetCore.Identity;

namespace SantasWishList.Data.Models;

public class Role : IdentityRole<int>, IEntity
{
    public Role(string role) : base(role) { }

    public Role() { }
}