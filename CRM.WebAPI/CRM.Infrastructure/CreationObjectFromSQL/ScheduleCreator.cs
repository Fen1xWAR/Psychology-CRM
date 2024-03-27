using System.Data;
using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class ScheduleCreator:ICreator<Schedule>
{
    public Schedule Map(NpgsqlDataReader reader)
    {
        return new Schedule()
        {
            ScheduleId = reader.GetGuid(0),
            PsychologistId = reader.GetGuid(1),
            WorkDay = reader.GetFieldValue<DateOnly>(2),
            StartTime = reader.GetFieldValue<TimeOnly>(3),
            EndTime = reader.GetFieldValue<TimeOnly>(4)

        };
    }
}