using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;

namespace Framework.App.Services;

public class ProductsService : BaseService<Product, long>, IProductsService
{
    public ProductsService(IBaseRepository<Product, long> repo) : base(repo)
    {
    }

    
}