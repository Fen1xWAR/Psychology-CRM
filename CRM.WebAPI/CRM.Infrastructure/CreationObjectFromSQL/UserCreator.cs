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
            Email = reader.GetString(1),
            Password =reader.GetString(2),
            Role =reader.GetString(3),
            ContactId = reader.GetGuid(4)
        };

    }
}