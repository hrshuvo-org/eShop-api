using System.Text.Json.Serialization;
using Framework.Core.Data;
using Framework.Core.Repositories;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;
using Framework.Core.Services.Interfaces;
using Framework.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Extensions;

public static class AppExtension
{
    public static async Task AddApplicationCoreServices(this IServiceCollection services, IConfiguration config)
    {
        #region Common

        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4201","http://localhost:4200");
            });
        });

        #endregion
        
        #region Database

        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseMySQL(config.GetConnectionString("DefaultConnection")!, m => m.MigrationsAssembly("API"));
        });

        #endregion

        #region DI
        services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped(typeof(IBaseService<,>), typeof(BaseService<,>));
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

        #endregion

        #region Extend ApplicationServices

        services.AddIdentityServices(config);
        await services.AddApplicationServices(config);
        

        #endregion
    }
}