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

    #region Categories

    public async Task<CategoryDto> GetCategoryAsync(long id)
    {
        var category = await GetAsync(id);

        if (category is null)
            throw new AppException(404, "Category not found");


        var dto = new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            Status = category.Status,
            ParentCategoryId = category.ParentCategoryId,
            Variations = await LoadVariations(category.Id)
        };


        return dto;
    }


    public async Task Save(CategoryDto dto)
    {
        var transaction = await BeginTransaction();

        try
        {
            var categoryExists = await GetAsync(dto.Name);

            if (categoryExists is not null &&
                categoryExists.Id != dto.Id)
                throw new AppException($"Category {dto.Name} already exists");

            var entity = new Category()
            {
                Id = dto.Id ?? 0,
                Name = dto.Name,
                Status = dto.Status,
                ParentCategoryId = dto.ParentCategoryId ?? dto.ParentCategoryId!.Value
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

                    var variationOptionExists =
                        await _variationOptionService.GetAsync(variationOptionExistsQuery, false, true);

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
                Children = BuildCategoryHierarchy(categories, c.Id) // Recursively build the hierarchy for each child
            })
            .ToList();
    }
    
    public async Task<List<CategorySelectDto>> GetCategorySelectList()
    {
        Expression<Func<Category, bool>> query = c => c.Status == EntityStatus.Active;

        var categories = await LoadAsync(query);

        var categorySelectList = BuildCategorySelectHierarchy(categories, 0);

        return categorySelectList;
    }

    private static List<CategorySelectDto> BuildCategorySelectHierarchy(List<Category> categories, long parentId)
    {
        return categories
            .Where(c => c.ParentCategoryId == parentId) // Find all categories with the current parentId
            .Select(c => new CategorySelectDto()
            {
                Id = c.Id,
                Label = c.Name,
                Children = BuildCategorySelectHierarchy(categories, c.Id) // Recursively build the hierarchy for each child
            })
            .ToList();
    }

    #endregion


    #region Variations

    public async Task<List<VariationDto>> LoadVariations(long categoryId)
    {
        var variations =
            await _variationService.LoadAsync(v => v.CategoryId == categoryId && v.Status == EntityStatus.Active);
        var variationDtos = new List<VariationDto>();

        foreach (var v in variations)
        {
            var variationDto = new VariationDto()
            {
                Id = v.Id,
                Name = v.Name,
                // CategoryId = v.CategoryId,
                VariationOptions = await LoadVariationOptions(v.Id)
            };
            variationDtos.Add(variationDto);
        }

        return variationDtos;
    }


    public async Task<List<VariationOptionDto>> LoadVariationOptions(long argId)
    {
        var variationOptions =
            await _variationOptionService.LoadAsync(vo => vo.VariationId == argId && vo.Status == EntityStatus.Active);

        return variationOptions.Select(vo => new VariationOptionDto()
        {
            Id = vo.Id,
            Value = vo.Value,
            VariationId = vo.VariationId
        }).ToList();
    }

    #endregion
}