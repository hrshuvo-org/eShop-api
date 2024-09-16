using Framework.Core.Controllers;
using Framework.Core.Helpers.Pagination;
using Framework.Core.Models.Dtos;
using Framework.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] ListParams param)
    {
        var result = await _userService.LoadAsync(param.Query, param.PageNumber, param.PageSize, param.Status, param.WithDeleted);

        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(long id)
    {
        var user = await _userService.GetAsync(id);
        
        if (user is null)
            return NotFound("User not found");
        
        return Ok(user);
    }
}