using System.Data;
using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;
public class PsychologistCreator : ICreator<Psychologist>
{
    public Psychologist Map(NpgsqlDataReader reader) //сборщик для клиента)
    {
        return new Psychologist()
        {
            PsychologistId = reader.GetGuid(0),
            
            UserId = reader.GetGuid(1),
            About = reader.GetString(2)
        };
    }
}

