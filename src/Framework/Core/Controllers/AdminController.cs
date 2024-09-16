using Framework.Core.Helpers.Pagination;
using Framework.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers;

public class AdminController : BaseApiController
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles([FromQuery] ListParams param)
    {
        var users = await _userService.LoadUserWithRolesAsync(param.Query, param.PageNumber, param.PageSize,
            param.Status, param.WithDeleted);

        return Ok(users);
    }
}