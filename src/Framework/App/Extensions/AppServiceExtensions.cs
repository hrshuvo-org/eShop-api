using Framework.App.Services;
using Framework.App.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.App.Extensions;

public static class AppServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {

        #region App Servies
        // added to applicationServiceExtension

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IProductItemService, ProductItemService>();


        #endregion
    }
}