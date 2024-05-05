using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using Microsoft.AspNetCore.Http;

namespace CRM.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<IOperationResult<string>> Login(UserAuth model);
    Task<IOperationResult<string>> Register(UserRegModel regModel);
    UserBase? GetCurrentUser(HttpContext user);
}