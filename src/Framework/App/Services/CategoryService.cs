using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Exceptions;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;

namespace Framework.App.Services;

public class CategoryService : BaseService<Category, long>, ICategoryService
{
    public CategoryService(IBaseRepository<Category, long> repo) : base(repo)
    {
    }

    public async Task Save(CategoryDto dto)
    {
        var categoryExists =  await GetAsync(dto.Name);
        
        if(categoryExists is not null && 
           categoryExists.Id != dto.Id && 
           categoryExists.ParentCategoryId == dto.ParentCategoryId)
            throw new AppException($"Category {dto.Name} already exists");
        

        var entity = new Category()
        {
            Id = dto.Id ?? 0,
            Name = dto.Name,
            Status = dto.Status,
            ParentCategoryId = dto.ParentCategoryId!.Value
        };
        
        if (dto.ParentCategoryId is not null and not 0)
        {
            var parentCategory = await GetAsync(dto.ParentCategoryId.Value);
            entity.ParentCategory = parentCategory.Name;
        }

        if (entity.Id == 0)
        {
            await AddAsync(entity);
        }
        else
        {
            await UpdateAsync(entity);
        }
    }
}