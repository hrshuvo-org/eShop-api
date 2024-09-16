using Framework.Core.Models.Dtos;

namespace Framework.Core.Services.Interfaces;

public interface IAuthService
{
    Task<UserLoginDto> Register(RegisterDto model);
    Task<UserLoginDto> Login(LoginDto loginDto);

}