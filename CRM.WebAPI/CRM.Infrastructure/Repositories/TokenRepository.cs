using System.Globalization;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace CRM.Infrastructure.Repositories;

public class TokenRepository : RepositoryBase, ITokenRepository
{
    public TokenRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<Token>> GetTokenByUserAndDeviceId(Guid userId, Guid deviceId)
    {
        var result =
            (await GetDataSql<Token, TokenCreator>(
                "SELECT * FROM tokens WHERE user_id = @userId and device_id = @deviceId",
                new NpgsqlParameter("@userId", userId), new NpgsqlParameter("@deviceId", deviceId))).FirstOrDefault();
        if (result == null)
        {
            return new ElementNotFound<Token>(null, "TokenNotFound");
        }

        return new Success<Token>(result);
    }

    public async Task<IOperationResult> WriteTokenAsync(TokenModel tokenModel)
    {
        if (tokenModel.UserId == Guid.Empty || tokenModel.RefreshToken == "" || tokenModel.DeviceId == Guid.Empty)
            return new ConflictResult("Empty input is invalid");
        var tokenId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT into tokens (token_id, user_id, refresh_token, expired_date,device_id) values (@tokenId, @userId, @refreshToken,@expiredDate, @deviceId)",
            new NpgsqlParameter("@tokenId", tokenId), new NpgsqlParameter("@userId", tokenModel.UserId),
            new NpgsqlParameter("@refreshToken", tokenModel.RefreshToken),
            new NpgsqlParameter("@expiredDate", tokenModel.ExpiredDateTime),
            new NpgsqlParameter("@deviceId", tokenModel.DeviceId));
        return new Success();
    }

    public async Task<IOperationResult> UpdateToken(Token tokenModel)
    {
        if (tokenModel.TokenId == Guid.Empty || tokenModel.UserId == Guid.Empty || tokenModel.RefreshToken == "" ||
            tokenModel.DeviceId == Guid.Empty)
            return new ConflictResult("Empty input is invalid");

        await ExecuteSql(
            "UPDATE tokens set user_id = @userId, refresh_" +
            "token = @refreshToken, expired_date = @expiredDate, device_id = @deviceId WHERE token_id = @tokenId",
            new NpgsqlParameter("@userId", tokenModel.UserId),
            new NpgsqlParameter("@refreshToken", tokenModel.RefreshToken),
            new NpgsqlParameter("@tokenId", tokenModel.TokenId),
            new NpgsqlParameter("@expiredDate", tokenModel.ExpiredDateTime),
            new NpgsqlParameter("@deviceId", tokenModel.DeviceId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveAllUserTokens(Guid userId)
    {
        await ExecuteSql("DELETE FROM tokens where user_id = @userId", new NpgsqlParameter("@userId", userId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveTokenByUserAndDeviceId(Guid userId, Guid deviceId)
    {
        Log.Logger.Warning($"Deleting with {userId} and {deviceId}");
        await ExecuteSql("DELETE FROM tokens where user_id = @userId and device_id = @deviceId",
            new NpgsqlParameter("@userId", userId), new NpgsqlParameter("@deviceID", deviceId));
        return new Success();
    }
}