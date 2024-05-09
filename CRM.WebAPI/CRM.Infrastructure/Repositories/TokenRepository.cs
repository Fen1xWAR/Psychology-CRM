using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class TokenRepository : RepositoryBase, ITokenRepository
{
    public TokenRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<Token>> GetTokenByUserId(Guid userId)
    {
        var result =
            (await GetDataSql<Token, TokenCreator>("SELECT * FROM tokens WHERE user_id = @a",
                new NpgsqlParameter("@a", userId))).FirstOrDefault();
        if (result == null)
        {
            return new ElementNotFound<Token>(null, "TokenNotFound");
        }

        return new Success<Token>(result);
    }

    public async Task<IOperationResult> WriteTokenAsync(TokenModel tokenModel)
    {
        if (tokenModel.userId == Guid.Empty || tokenModel.RefreshToken == "")
            return new ConflictResult("Empty input is invalid");
        var tokenId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT into tokens (token_id, user_id, refresh_token) values (@tokenId, @userId, @refreshToken)",
            new NpgsqlParameter("@tokenId", tokenId), new NpgsqlParameter("@userId", tokenModel.userId),
            new NpgsqlParameter("@refreshToken", tokenModel.RefreshToken));
        return new Success();
    }

    public async Task<IOperationResult> UpdateToken(TokenModel tokenModel)
    {
        if (tokenModel.userId == Guid.Empty || tokenModel.RefreshToken == "")
            return new ConflictResult("Empty input is invalid");
        var tokenToUpdate = (await this.GetTokenByUserId(tokenModel.userId)).Result;
        await ExecuteSql("UPDATE tokens set user_id = @userId, refresh_token = @refreshToken WHERE token_id = @tokenId",
            new NpgsqlParameter("@userId", tokenModel.userId),
            new NpgsqlParameter("@refreshToken", tokenModel.RefreshToken),
            new NpgsqlParameter("@tokenId", tokenToUpdate.TokenId));
        return new Success();
    }
}