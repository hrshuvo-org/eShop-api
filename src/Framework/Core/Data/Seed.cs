using System.Text.Json;
using Framework.Core.Auth;
using Framework.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Framework.Core.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var directory = Directory.GetCurrentDirectory() + "/../Framework/Core/Data/UserSeedData.json";
        var userData = await File.ReadAllTextAsync(directory);
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

        var roles = new List<AppRole>()
        {
            new() { Name = AppRoles.Member },
            new() { Name = AppRoles.Administrator },
            new() { Name = AppRoles.SuperAdmin},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        if (users == null) return;

        foreach (var user in users)
        {
            user.UserName = user.UserName == null ? user.Email!.ToLower() : user.UserName.ToLower();

            var success = await userManager.CreateAsync(user, "1234");
            if (!success.Succeeded) continue;
        
            await userManager.AddToRoleAsync(user, AppRoles.Member);
        }
    
        var admin = new AppUser()
        {
            UserName = "admin"
        };
    
        await userManager.CreateAsync(admin, "1234");
        await userManager.AddToRolesAsync(admin, new[] {AppRoles.SuperAdmin, AppRoles.Administrator, AppRoles.Member});
    }
}