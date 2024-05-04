using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class ScheduleRepository : RepositoryBase, IScheduleRepository
{
    public ScheduleRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Schedule>> GetAll()
    {
        return await GetDataSql<Schedule, ScheduleCreator>("SELECT * FROM schedules");
    }

    public async Task<Schedule> GetById(Guid id)
    {
        return (await GetDataSql<Schedule, ScheduleCreator>("SELECT * FROM schedules WHERE schedule_id = @id",
            new NpgsqlParameter("@id", id))).First();
    }

    public async Task Put(Schedule schedule)
    {
        var scheduleId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO schedules (schedule_id, psychologist_id, work_day, start_time, end_time) VALUES (@id, @psychologistId, @workDay, @startTime, @endTime)",
            new NpgsqlParameter("@id", scheduleId),
            new NpgsqlParameter("@psychologistId", schedule.PsychologistId),
            new NpgsqlParameter("@workDay", schedule.WorkDay),
            new NpgsqlParameter("@startTime", schedule.StartTime),
            new NpgsqlParameter("@endTime", schedule.EndTime));
    }

    public async Task Update(Schedule dataToUpdate)
    {
        await ExecuteSql(
            "UPDATE schedules SET psychologist_id = COALESCE(@psychologistId, psychologist_id), work_day = COALESCE(@workDay, work_day), start_time = COALESCE(@startTime, start_time), end_time = COALESCE(@endTime, end_time) WHERE schedule_id = @id",
            new NpgsqlParameter("@psychologistId", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@workDay", dataToUpdate.WorkDay),
            new NpgsqlParameter("@startTime", dataToUpdate.StartTime),
            new NpgsqlParameter("@endTime", dataToUpdate.EndTime),
            new NpgsqlParameter("@id", dataToUpdate.ScheduleId));
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql("DELETE FROM schedules WHERE schedule_id = @id", new NpgsqlParameter("@id", id));
    }
}