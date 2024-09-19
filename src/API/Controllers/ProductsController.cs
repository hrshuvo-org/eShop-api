using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Controllers;
using Framework.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IProductsService _productsService;
    private readonly IProductItemService _productItemService;

    public ProductsController(IProductsService productsService, 
        IProductItemService productItemService)
    {
        _productsService = productsService;
        _productItemService = productItemService;
    }
    
    
    #region Product
    
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ListParams param)
    {
        var result = await _productsService.LoadAsync(param.Query, param.PageNumber, param.PageSize, param.Status, param.WithDeleted);

        var dataToReturn = new Pagination<Product>(result.CurrentPage, result.PageSize, result.TotalCount,
            result.TotalPage, result);
        
        return Ok(dataToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(long id)
    {
        var user = await _productsService.GetAsync(id);
        
        if (user is null)
            return NotFound("Product not found");
        
        return Ok(user);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> SaveProduct(Product product)
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
    public async Task<IActionResult> DeleteProduct(long id)
    {
        var category = await _productsService.GetAsync(id);
        if (category is null)
            return NotFound("Product not found");
        
        await _productsService.DeleteAsync(id);
        
        return Ok("Product deleted");
    }
    
    #endregion
    
    

    #region Product Item

    [HttpGet("items")]
    public async Task<IActionResult> GetProductItems([FromQuery] ListParams param)
    {
        var result = await _productItemService.LoadAsync(param.Query, param.PageNumber, param.PageSize, param.Status, param.WithDeleted);

        var dataToReturn = new Pagination<ProductItem>(result.CurrentPage, result.PageSize, result.TotalCount,
            result.TotalPage, result);
        
        return Ok(dataToReturn);
    }

    [HttpGet("items/{id}")]
    public async Task<IActionResult> GetProductItem(long id)
    {
        var user = await _productItemService.GetAsync(id);
        
        if (user is null)
            return NotFound("Product Item not found");
        
        return Ok(user);
    }
    
    [HttpPost("items/save")]
    public async Task<IActionResult> SaveProductItem(ProductItem item)
    {
        var itemExists = await _productItemService.GetAsync(item.Name);

        if (itemExists is not null && itemExists.Id != item.Id)
        {
            return BadRequest("Product item already exists");
        }
        
        if(item.Id == 0)
        {
            await _productItemService.AddAsync(item);
        }
        else
        {
            await _productItemService.UpdateAsync(item);
        }

        return Ok();
    }

    [HttpDelete("items/delete/{id}")]
    public async Task<IActionResult> DeleteProductItem(long id)
    {
        var category = await _productItemService.GetAsync(id);
        if (category is null)
            return NotFound("Product item not found");
        
        await _productsService.DeleteAsync(id);
        
        return Ok("Product item deleted");
    }


    #endregion
    
    
}