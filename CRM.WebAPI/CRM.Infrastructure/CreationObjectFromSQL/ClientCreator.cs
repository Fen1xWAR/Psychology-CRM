using System.Data;
using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class ClientCreator : ICreator<Client>
{
    public Client Map(NpgsqlDataReader reader)
    {
        return new Client()
        {
            ClientId = reader.GetGuid(0),
            Name = reader.GetString(1),
            Lastname = reader.GetString(2),
            Form = reader.GetGuid(3),
            CurrentProblem = reader.GetString(4),
            ContactId = reader.GetGuid(5)
        };
    }
}   