using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;

namespace Framework.App.Services;

public class ProductItemService : BaseService<ProductItem, long>, IProductItemService
{
    public ProductItemService(IBaseRepository<ProductItem, long> repo) : base(repo)
    {
    }
}