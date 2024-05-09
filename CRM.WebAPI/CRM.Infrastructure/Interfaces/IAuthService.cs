using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using Microsoft.AspNetCore.Http;

namespace CRM.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<IOperationResult<Tokens>> Login(UserAuth model);
    Task<IOperationResult<Tokens>> Register(UserRegModel regModel);
    Task<IOperationResult<Tokens>> RefreshTokens(string token, HttpContext context);
    UserBase? GetCurrentUser(HttpContext user);
}