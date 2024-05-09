using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface ITokenRepository
{
    Task<IOperationResult<Token>> GetTokenByUserId(Guid userId);
    Task<IOperationResult> WriteTokenAsync(TokenModel tokenModel);
    Task<IOperationResult> UpdateToken(TokenModel tokenModel);
}