using Framework.App.Models.Dtos;
using Framework.App.Models.Entities;
using Framework.App.Services.Interfaces;
using Framework.Core.Controllers;
using Framework.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
       _categoryService = categoryService;
    }

    #region Categories

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] ListParams param)
    {
        var result = await _categoryService.LoadAsync(param.Query, param.PageNumber, param.PageSize, param.Status, param.WithDeleted);

        var dataToReturn = new Pagination<Category>(result.CurrentPage, result.PageSize, result.TotalCount,
            result.TotalPage, result);
        
        return Ok(dataToReturn);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCategory(long id)
    {
        var user = await _categoryService.GetCategoryAsync(id);
        
        if (user is null)
            return NotFound("Category not found");
        
        return Ok(user);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveCategory(CategoryDto category)
    {
        await _categoryService.Save(category);

        return Ok();
    }

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        var category = await _categoryService.GetAsync(id);
        if (category is null)
            return NotFound("Category not found");
        
        await _categoryService.DeleteAsync(id);
        
        return Ok("Category deleted");
    }
    
    [HttpGet("tree")]
    public async Task<IActionResult> GetCategoryGroups()
    {
        var result = await _categoryService.GetCategoryGroups();
        
        return Ok(result);
    }
    
    [HttpGet("select-list")]
    public async Task<IActionResult> GetCategorySelectList()
    {
        var result = await _categoryService.GetCategorySelectList();
        
        return Ok(result);
    }

    #endregion


    #region Variations

    [HttpGet("variations/{categoryId:long}")]
    public async Task<IActionResult> GetVariations(long categoryId)
    {
        var result = await _categoryService.LoadVariations(categoryId);

        return Ok(result);
    }
    
    [HttpGet("variation-options/{variationId:long}")]
    public async Task<IActionResult> GetVariationOptions(long variationId)
    {
        var result = await _categoryService.LoadVariationOptions(variationId);

        return Ok(result);
    }

    #endregion
    
    
    
    
}