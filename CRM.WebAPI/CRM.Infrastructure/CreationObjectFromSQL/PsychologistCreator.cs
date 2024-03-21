using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;
public class PsychologistCreator : ICreator<Psychologist>
{
    public Psychologist Map(NpgsqlDataReader reader) //сборщик для клиента)
    {
        return new Psychologist()
        {
            //читаем собираем обьект:) ридер просим взять строку гуид или еще что то с определенного места (это номера колонок)
            PsychologistId = reader.GetGuid(0),
            Name = reader.GetString(1),
            Lastname = reader.GetString(2),
            ContactId = reader.GetGuid(3)
        };
    }
}

