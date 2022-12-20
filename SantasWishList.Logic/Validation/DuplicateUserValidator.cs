using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SantasWishList.Logic.Validation;

public class DuplicateUserValidator
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public DuplicateUserValidator(UserManager<IdentityUser> userManager) => _userManager = userManager;

    public async Task<ValidationResult> ValidateDuplicateUsers(List<string> names)
    {
        var existingUsers = new List<string>();  
        
        foreach (var name in names)
            if (await _userManager.FindByNameAsync(name) != null) existingUsers.Add(name);

        return !existingUsers.Any() ? 
            ValidationResult.Success :
            new ValidationResult("De volgende kinderen zijn al geregistreerd: " + string.Join(", ", existingUsers.ToArray()));
    }
}