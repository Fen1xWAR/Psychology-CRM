using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class UserCreator:ICreator<User>
{
    
    public User Map(NpgsqlDataReader reader)
    {
        return new User()
        {
            UserId =reader.GetGuid(0),
            UserName =reader.GetString(1),
            Password =reader.GetString(2),
            Role =reader.GetString(3),
            DataId =reader.GetGuid(4)
        };

    }
}