using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Exceptions;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;
using Microsoft.AspNetCore.Http;

namespace Framework.App.Services;

public class ProductItemService : BaseService<ProductItem, long>, IProductItemService
{
    private readonly IPhotoService _photoService;
    
    public ProductItemService(IBaseRepository<ProductItem, long> repo, IPhotoService photoService) : base(repo)
    {
        _photoService = photoService;
    }

    public async Task AddPhotoAsync(IFormFile file, long productItemId)
    {
        var item = await GetAsync(productItemId);
        if (item is null)
            throw new AppException(404, "Product item not found");
        
        var result = await _photoService.AddPhotoAsync(file);
        
        if(result.Error is not null)
            throw new AppException(result.Error.Message);

        var photo = new Photo()
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        
        var itemPhotos = await _photoService.LoadAsync(i => i.ProductItemId == productItemId);
        var photoExists = itemPhotos.Find(p => p.IsMain);

        if (photoExists is null)
        {
            photo.IsMain = true;
            item.PhotoUrl = photo.Url;
        }
        
        item.Photos.Add(photo);
        await UpdateAsync(item);

    }
    
    
    
    
}