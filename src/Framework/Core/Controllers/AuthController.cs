using Framework.Core.Models.Dtos;
using Framework.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserLoginDto>> Register(RegisterDto registerDto)
    {
        var result = await _authService.Register(registerDto);

        return result;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginDto>> Login(LoginDto loginDto)
    {
        var result = await _authService.Login(loginDto);

        return result;
    }
}