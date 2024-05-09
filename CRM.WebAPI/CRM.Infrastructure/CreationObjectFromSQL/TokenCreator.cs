using System.Data;
using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class TokenCreator : ICreator<Token>
{
    public Token Map(NpgsqlDataReader reader)
    {
        return new Token
        {
            TokenId = reader.GetGuid(0),
            UserId = reader.GetGuid(1),
            RefreshToken = reader.GetString(2)
        };
    }
}