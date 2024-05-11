using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface ITokenRepository
{
    Task<IOperationResult<Token>> GetTokenByUserAndDeviceId(Guid userId, Guid deviceId);
    Task<IOperationResult> WriteTokenAsync(TokenModel tokenModel);
    Task<IOperationResult> UpdateToken(Token tokenModel);
    Task<IOperationResult> RemoveAllUserTokens(Guid userId);
    Task<IOperationResult> RemoveTokenByUserAndDeviceId(Guid userId, Guid deviceId);
}