using CRM.Core.Interfaces;
using CRM.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace CRM.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<IOperationResult<string>> Login(UserAuth model);
    UserBase? GetCurrentUser(HttpContext user);
}