using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRM.Domain.Models;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace CRM.WebAPI.Services;

public class AuthService : IAuthService
{
    private IUserRepository _userRepository;
    private IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<IOperationResult<string>> Login(UserAuth model)
    {
        var user = await _userRepository.GetUserByEmail(model.Email);

        if (!user.Successful || (user.Result.Password != model.Password))
            return new ConflictResult<string>(null, "Invalid login or password");
        return new Success<string>(GenerateTokenAsync(user.Result));

    }
    
     // public async Task<IOperationResult<string>> Register()

    private string GenerateTokenAsync(User userData)
    {
        
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userData.UserId.ToString()),
            new Claim(ClaimTypes.Role, userData.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthOptions:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["AuthOptions:Issuer"],
            audience: _configuration["AuthOptions:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public UserBase? GetCurrentUser(HttpContext user)
    {
        var userIdClaim = user.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var roleClaim = user.User.FindFirst(c => c.Type == ClaimTypes.Role);

        if (userIdClaim == null || roleClaim == null)
            return null;
        return new UserBase()
        {
            UserId = Guid.Parse(userIdClaim.Value),
            Role = roleClaim.Value,
        };

    }
}