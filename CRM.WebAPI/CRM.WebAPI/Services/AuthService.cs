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
    private IUserRepository _repository;
    private IConfiguration _configuration;

    public AuthService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<IOperationResult<string>> GenerateTokenAsync(UserAuth model)
    {
        var user = await _repository.GetUserByEmail(model.Email);
       
        if (!user.Successful || (user.Result.Password != model.Password))
            return new ConflictResult<string>(null,"Invalid login or password") ;
        var userData = user.Result;
        var claims = new[]
        {
            new Claim("user_id", userData.UserId.ToString()),
            new Claim("email", userData.Email),
            new Claim("role", userData.Role)
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

        return new Success<string>( new JwtSecurityTokenHandler().WriteToken(token));
    }
}