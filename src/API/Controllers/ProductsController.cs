using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Controllers;
using Framework.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] ListParams param)
    {
        var result = await _productsService.LoadAsync(param.Query, param.PageNumber, param.PageSize, param.Status, param.WithDeleted);

        var dataToReturn = new Pagination<Product>(result.CurrentPage, result.PageSize, result.TotalCount,
            result.TotalPage, result);
        
        return Ok(dataToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(long id)
    {
        var user = await _productsService.GetAsync(id);
        
        if (user is null)
            return NotFound("Product not found");
        
        return Ok(user);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> SaveCategory(Product product)
    {
        var productExists = await _productsService.GetAsync(product.Name);

        if (productExists is not null && productExists.Id != product.Id)
        {
            return BadRequest("Product already exists");
        }
        
        if(product.Id == 0)
        {
            await _productsService.AddAsync(product);
        }
        else
        {
            await _productsService.UpdateAsync(product);
        }

        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        var category = await _productsService.GetAsync(id);
        if (category is null)
            return NotFound("Product not found");
        
        await _productsService.DeleteAsync(id);
        
        return Ok("Product deleted");
    }
}