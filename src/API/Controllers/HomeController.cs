using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Controllers;
using Framework.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class HomeController : BaseApiController
{
    private readonly IProductItemService _productItemService;
    private readonly ISearchService _searchService;
    
    public HomeController(ISearchService searchService, IProductItemService productItemService)
    {
        _searchService = searchService;
        _productItemService = productItemService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ListParams param)
    {
        var result = await _productItemService.LoadAsync(param.Query);
    
        var dataToReturn = new Pagination<ProductItem>(result.CurrentPage, result.PageSize, result.TotalCount,
            result.TotalPage, result);
        
        return Ok(dataToReturn);
    }
    
    [HttpGet("items")]
    public async Task<IActionResult> Search([FromQuery] ListParams param, [FromQuery] long[] categoryIds, [FromQuery] long[] variationIds, [FromQuery] long[] optionIds)
    {
        var result = await _searchService.SearchAsync(param, categoryIds, variationIds, optionIds);
        
        return Ok(result);
    }
}