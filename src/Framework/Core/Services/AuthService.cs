using Framework.Core.Auth;
using Framework.Core.Exceptions;
using Framework.Core.Models.Dtos;
using Framework.Core.Models.Entities;
using Framework.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Framework.Core.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }


    public async Task<UserLoginDto> Register(RegisterDto registerDto)
    {
        var userExists = await UserExists(registerDto.Email);
        if (userExists)
            throw new AppException(400, "Username is in use");

        var user = new AppUser()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Name = registerDto.Email
        };
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        
        if(!result.Succeeded) 
            throw new AppException(400, "Something went wrong, please try later!"); 
            
        var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.Member); 
            
        if(!roleResult.Succeeded)
            throw new AppException(400, "Something went wrong, please try later!");
        
        return new UserLoginDto()
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            Displayname = user.Name!
        };
    }

    
    public async Task<UserLoginDto> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(x =>
            x.UserName == loginDto.UserName || x.Email == loginDto.UserName);
        
        if (user == null) throw new AppException(401, "Invalid username");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if(!result) throw new AppException(401, "Invalid password");

        var token = await _tokenService.CreateToken(user);
        
        return new UserLoginDto()
        {
            Username = user.UserName,
            Displayname = user.Name,
            Token = token
        };
    }















    #region Private

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName != null && x.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
    }

    #endregion
}