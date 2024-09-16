using Framework.Core.Models;
using Framework.Core.Models.Entities;

namespace Framework.Core.Services.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}