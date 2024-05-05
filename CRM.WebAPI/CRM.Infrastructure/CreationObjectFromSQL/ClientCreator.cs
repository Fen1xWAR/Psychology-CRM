using System.Data;
using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class ClientCreator : ICreator<Client>
{
    public Client Map(NpgsqlDataReader reader) //сборщик для клиента)
    {
        return new Client()
        {
            //читаем собираем обьект:) ридер просим взять строку гуид или еще что то с определенного места (это номера колонок)
            ClientId = reader.GetGuid(0),
            FormId = reader.GetGuid(1),
            CurrentProblem = reader.GetString(2),
            ContactId = reader.GetGuid(3),
            UserId = reader.GetGuid(4)
        };
    }
}