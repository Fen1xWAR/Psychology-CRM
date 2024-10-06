using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class VisitCreator : ICreator<Visit>
{
    public Visit Map(NpgsqlDataReader reader)
    {
        return new Visit
        {
            VisitId = reader.GetGuid(0),
            ClientId = reader.GetGuid(1),
            ClientNote = reader.GetString(2),
            PsychologistDescription = reader.GetString(3),
            ServiceId = reader.GetGuid(4),
            PsychologistId = reader.GetGuid(5),
            ScheduleId = reader.GetGuid(6)
        };
    }
}