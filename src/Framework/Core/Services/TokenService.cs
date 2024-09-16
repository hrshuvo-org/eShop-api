using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Framework.Core.Models;
using Framework.Core.Models.Entities;
using Framework.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Framework.Core.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SymmetricSecurityKey _key;
    
    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]!));
    }
    
    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            // new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            // new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
        };
    
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
    
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = credentials
        };
    
        var tokenHandler = new JwtSecurityTokenHandler();
    
        var token = tokenHandler.CreateToken(tokenDescriptor);


        return tokenHandler.WriteToken(token);
    }
    
}