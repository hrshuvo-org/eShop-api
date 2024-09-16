using Microsoft.AspNetCore.Identity;

namespace Framework.Core.Models.Entities;

public class AppRole : IdentityRole<long>
{
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}