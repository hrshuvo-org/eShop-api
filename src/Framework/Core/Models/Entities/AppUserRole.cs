using Microsoft.AspNetCore.Identity;

namespace Framework.Core.Models.Entities;

public class AppUserRole : IdentityUserRole<long>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}