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
            DateTime = reader.GetDateTime(2),
            ClientNote = reader.GetString(3),
            PsychologistDescription = reader.GetString(4),
            ServiceId = reader.GetGuid(5),
            PsychologistId = reader.GetGuid(6),
        };
    }
}