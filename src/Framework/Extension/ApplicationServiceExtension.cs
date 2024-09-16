using Framework.App.Extensions;
using Framework.Core.Data;
using Framework.Core.Helpers;
using Framework.Core.Models.Entities;
using Framework.Repositories;
using Framework.Repositories.Interfaces;
using Framework.Services;
using Framework.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Extension;

public static class ApplicationServiceExtension
{
    public static async Task AddApplicationServices(this IServiceCollection services)
    {
        #region Dependency Injection
        services.AddAutoMapper(typeof(MappingProfiles));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();



        #endregion


        #region Seed Data

        try
        {
            var context = services.BuildServiceProvider().GetRequiredService<DataContext>();
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<AppRole>>();
            await context.Database.MigrateAsync();
            // await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]"); // MSSQL
            // await context.Database.ExecuteSqlRawAsync("DELETE FROM `Connections`"); // MYSQL
            await Seed.SeedUsers(userManager, roleManager);
        }
        catch (Exception e)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<DataContext>>();
            logger.LogError(e, "An error occurred during migration");
        }

        #endregion
        
        
        AppServiceExtensions.AddApplicationServices(services);

    }
}