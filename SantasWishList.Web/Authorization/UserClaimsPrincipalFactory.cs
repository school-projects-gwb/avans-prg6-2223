using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SantasWishList.Data.Models;

namespace SantasWishList.Web.Authorization;

public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
{
    public UserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }
    
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(
            new Claim("IsGood", user.IsGood.ToString() ?? "-1", ClaimValueTypes.Integer));

        return identity;
    }
}