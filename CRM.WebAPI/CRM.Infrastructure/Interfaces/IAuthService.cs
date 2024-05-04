using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IAuthService
{
    Task<IOperationResult<string>> GenerateTokenAsync(UserAuth model);
}