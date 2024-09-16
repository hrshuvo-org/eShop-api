using Framework.App.Models.Entities;
using Framework.Core.Services.Interfaces;

namespace Framework.App.Services.Interfaces;

public interface IProductsService : IBaseService<Product, long>
{
    // Task CreateAsync(Product entity);
}