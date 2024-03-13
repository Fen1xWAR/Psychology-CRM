using System.Data;
using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class ClientCreator : ICreator<Client>
{
    public Client Map(NpgsqlDataReader reader) //сборщик для клиента)
    {
        return new Client()
        { //читаем собираем обьект:) ридер просим взять строку гуид или еще что то с определенного места (это номера колонок)
            ClientId = reader.GetGuid(0),
            Name = reader.GetString(1),
            Lastname = reader.GetString(2),
            Form = reader.GetGuid(3),
            CurrentProblem = reader.GetString(4),
            ContactId = reader.GetGuid(5)
        };
    }
}   