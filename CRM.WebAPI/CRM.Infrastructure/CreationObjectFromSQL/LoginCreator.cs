using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class LoginCreator: ICreator<Login>
{
    public Login Map(NpgsqlDataReader reader)
    {
        return new Login()
        {
            LoginId = reader.GetGuid(0),
            UserId = reader.GetGuid(1),
            LoginTime = reader.GetDateTime(2),
            LogoutTime = reader.GetDateTime(3)
        };
    }
}