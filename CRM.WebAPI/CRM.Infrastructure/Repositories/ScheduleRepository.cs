using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class ScheduleRepository:RepositoryBase,IScheduleRepository
{
    public ScheduleRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Schedule>> GetAll()
    {
        return
            await GetDataSql<Schedule, ScheduleCreator>(
                "SELECT * FROM schedules"); 
    }

    public async Task<Schedule> GetById(Guid id)
    {
        return (await GetDataSql<Schedule, ScheduleCreator>
            ($"SELECT * FROM schedules WHERE schedule_id = '{id}'")).First();
    }

    public async Task Put(Schedule schedule)
    {
        var scheduleId = Guid.NewGuid(); 
        await ExecuteSql($"INSERT INTO schedules (schedule_id, psychologist_id, work_day, start_time, end_time" +
                         $") VALUES ('{scheduleId}','{schedule.PsychologistId}','{schedule.WorkDay:yyyy-MM-dd}','{schedule.StartTime:HH:mm:ss.fff}','{schedule.EndTime:HH:mm:ss.fff}')");
    }

    public async Task Update(Schedule dataToUpdate)
    {
        await ExecuteSql($"UPDATE schedules SET" +
                         $" psychologist_id = coalesce('{dataToUpdate.PsychologistId}', psychologist_id), " +
                         $"work_day = coalesce('{dataToUpdate.WorkDay:yyyy-MM-dd}', work_day), " +
                         $"start_time = coalesce('{dataToUpdate.StartTime:HH:mm:ss.fff}',start_time) ," +
                         $"end_time = coalesce('{dataToUpdate.EndTime:HH:mm:ss.fff}',end_time) " +
                         $"WHERE schedule_id = '{dataToUpdate.ScheduleId}'");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM schedules WHERE schedule_id = '{id}'");
    }
}