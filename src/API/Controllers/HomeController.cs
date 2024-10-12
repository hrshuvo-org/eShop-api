using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Controllers;
using Framework.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class HomeController : BaseApiController
{
    private readonly IProductsService _productsService;
    private readonly IProductItemService _productItemService;
    private readonly ICategoryService _categoryService;

    public HomeController(IProductsService productsService, ICategoryService categoryService, IProductItemService productItemService)
    {
        _productsService = productsService;
        _categoryService = categoryService;
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
}