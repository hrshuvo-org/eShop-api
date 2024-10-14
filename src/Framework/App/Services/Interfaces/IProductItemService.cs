using Framework.App.Models.Entities;
using Framework.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Framework.App.Services.Interfaces;

public interface IProductItemService : IBaseService<ProductItem, long>
{
    Task<Photo> AddPhotoAsync(IFormFile file, long productItemId);
}