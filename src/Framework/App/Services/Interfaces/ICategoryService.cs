using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.Core.Services.Interfaces;

namespace Framework.App.Services.Interfaces;

public interface ICategoryService : IBaseService<Category, long>
{
    Task Save(CategoryDto category);
}