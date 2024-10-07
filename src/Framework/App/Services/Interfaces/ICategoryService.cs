using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.Core.Services.Interfaces;

namespace Framework.App.Services.Interfaces;

public interface ICategoryService : IBaseService<Category, long>
{
    Task<CategoryDto> GetCategoryAsync(long id);
    Task Save(CategoryDto category);
    Task<List<CategoryGroupDto>> GetCategoryGroups();
    Task<List<CategorySelectDto>> GetCategorySelectList();
    
    Task<List<VariationDto>> LoadVariations(long categoryId);
    Task<List<VariationOptionDto>> LoadVariationOptions(long argId);
}