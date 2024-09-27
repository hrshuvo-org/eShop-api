using System.Linq.Expressions;
using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Exceptions;
using Framework.Core.Models;
using Framework.Core.Repositories.Interfaces;
using Framework.Core.Services;
using Framework.Core.Services.Interfaces;

namespace Framework.App.Services;

public class CategoryService : BaseService<Category, long>, ICategoryService
{
    private readonly IBaseService<Variation, long> _variationService;
    private readonly IBaseService<VariationOption, long> _variationOptionService;
    
    public CategoryService(IBaseRepository<Category, long> repo, 
        IBaseService<Variation, long> variationService, 
        IBaseService<VariationOption, long> variationOptionService) : base(repo)
    {
        _variationService = variationService;
        _variationOptionService = variationOptionService;
    }

    public async Task Save(CategoryDto dto)
    {
        var transaction = await BeginTransaction();
        
        try
        {
            var categoryExists =  await GetAsync(dto.Name);
        
            if(categoryExists is not null && 
               categoryExists.Id != dto.Id)
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
            
            
            #region variation

            foreach (var variation in dto.Variations)
            {
                var variationEntity = new Variation()
                {
                    Id = variation.Id,
                    Name = variation.Name,
                    CategoryId = entity.Id,
                    Category = entity.Name
                };
                
                Expression<Func<Variation, bool>> variationExistsQuery = v => 
                    v.Name.Equals(variation.Name, StringComparison.CurrentCultureIgnoreCase) && 
                    v.CategoryId == entity.Id || v.Id == variation.Id;
                
                var variationExists = await _variationService.GetAsync(variationExistsQuery, false, true);

                if (variationExists is null)
                {
                    await _variationService.AddAsync(variationEntity);
                }
                else
                {
                    variationEntity.Id = variationExists.Id;
                    await _variationService.UpdateAsync(variationEntity);
                }

                #region VariationOption

                foreach (var option in variation.VariationOptions)
                {
                    var variationOptionEntity = new VariationOption()
                    {
                        Id = option.Id,
                        Value = option.Value,
                        VariationId = variationEntity.Id,
                        Variation = variationEntity.Name
                    };
                    
                    Expression<Func<VariationOption, bool>> variationOptionExistsQuery = vo => 
                        vo.Value.Equals(option.Value, StringComparison.CurrentCultureIgnoreCase) &&
                        vo.VariationId == variationEntity.Id || vo.Id == option.Id;
                    
                    var variationOptionExists = await _variationOptionService.GetAsync(variationOptionExistsQuery, false, true);
                    
                    if (variationOptionExists is null)
                    {
                        await _variationOptionService.AddAsync(variationOptionEntity);
                    }
                    else
                    {
                        variationOptionEntity.Id = variationOptionExists.Id;
                        await _variationOptionService.UpdateAsync(variationOptionEntity);
                    }
                }

                #endregion

                
            }

            #endregion
            
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new AppException(e.Message);
        }
        
        
    }

    public async Task<List<CategoryGroupDto>> GetCategoryGroups()
    {
        Expression<Func<Category, bool>> query = c => c.Status == EntityStatus.Active;
        
        var categories = await LoadAsync(query);
        
        // var categoryGroups2 = categories
        //     .GroupBy(c => c.ParentCategoryId)
        //     .Select(g => new CategoryGroupDto()
        //     {
        //         Id = g.Key, 
        //         Name = categories.FirstOrDefault(c => c.Id == g.Key)?.Name,
        //         CategoryList = g.Select(c => new CategoryGroupDto()
        //         {
        //             Id = c.Id,
        //             Name = c.Name
        //         }).ToList()
        //     }).ToList();

        var categoryGroups = BuildCategoryHierarchy(categories, 0);
                
        return categoryGroups;
    }
    
    private List<CategoryGroupDto> BuildCategoryHierarchy(List<Category> categories, long? parentId = null)
    {
        return categories
            .Where(c => c.ParentCategoryId == parentId) // Find all categories with the current parentId
            .Select(c => new CategoryGroupDto()
            {
                Id = c.Id,
                Name = c.Name,
                CategoryList = BuildCategoryHierarchy(categories, c.Id) // Recursively build the hierarchy for each child
            })
            .ToList();
    }
        
}