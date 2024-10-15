using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.App.Models.ViewModels;
using Framework.App.Services.Interfaces;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Services.Interfaces;

namespace Framework.App.Services;

public class SearchService : ISearchService
{
    private readonly IProductsService _productsService;
    private readonly IProductItemService _productItemService;
    private readonly ICategoryService _categoryService;
    private readonly IBaseService<Variation, long> _variationService;
    private readonly IBaseService<VariationOption, long> _variationOptionService;

    public SearchService(IProductsService productsService, IProductItemService productItemService,
        ICategoryService categoryService, IBaseService<Variation, long> variationService,
        IBaseService<VariationOption, long> variationOptionService)
    {
        _productsService = productsService;
        _productItemService = productItemService;
        _categoryService = categoryService;
        _variationService = variationService;
        _variationOptionService = variationOptionService;
    }


    public async Task<SearchData> SearchAsync(ListParams param, long[] categoryIds, long[] variationIds, long[] optionIds)
    {
        var result = await _productItemService.LoadAsync(param.Query);
        var resultToReturn = new Pagination<ProductItem>(result.CurrentPage, result.PageSize, result.TotalCount,
            result.TotalPage, result);
        
        var productIdList = resultToReturn.Data.Select(x => x.ProductId).Distinct().ToList();
        var products = await _productsService.LoadAsync(p => productIdList.Contains(p.Id));
        
        var categoryIdList = products.Select(x => x.CategoryId).Distinct().ToList();
        var categories = await _categoryService.LoadAsync(p => categoryIdList.Contains(p.Id));
        
        var variationList = await _variationService.LoadAsync(v => 
            variationIds.Contains(v.Id) || 
            categoryIds.Contains(v.CategoryId) ||
            categoryIdList.Contains(v.CategoryId)
            );

        var variations = new List<VariationDto>();
        foreach (var v in variationList)
        {
            var variation = new VariationDto()
            {
                Id = v.Id,
                Name = v.Name,
                VariationOptions = await LoadVariationOptions(v.Id, optionIds)
            };
            
            variations.Add(variation);
        }
        
        var dataToReturn = new SearchData
        {
            Result = resultToReturn,
            Categories = categories,
            Variations = variations
        };

        return dataToReturn;
    }

    private async Task<List<VariationOptionDto>> LoadVariationOptions(long vId, long[] optionIds)
    {
        var options = await _variationOptionService.LoadAsync(vo => vo.VariationId == vId);
        
        var optionsToReturn = options.Select(vo => new VariationOptionDto
        {
            Id = vo.Id,
            Value = vo.Value
        }).ToList();

        return optionsToReturn;
    }
}