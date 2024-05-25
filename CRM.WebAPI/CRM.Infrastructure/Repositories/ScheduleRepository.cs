using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
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

    public async Task<IOperationResult<IEnumerable<Schedule>>> GetAll()
    {
        return new Success<IEnumerable<Schedule>>(
            await GetDataSql<Schedule, ScheduleCreator>("SELECT * FROM schedules"));
    }
    
    

    public async Task<IOperationResult<IEnumerable<Schedule>>> GetByPsychologistIdAndDay(Guid psychologistId,
        DateOnly day)
    {
        return new Success<IEnumerable<Schedule>>(await GetDataSql<Schedule, ScheduleCreator>(
            "SELECT * FROM schedules WHERE psychologist_id= @psychologistId and work_day = @day",
            new NpgsqlParameter("@psychologistId", psychologistId),
            new NpgsqlParameter("@day", day)));
    }

    public async Task<IOperationResult<Schedule>> GetById(Guid id)
    {
        var result = (await GetDataSql<Schedule, ScheduleCreator>("SELECT * FROM schedules WHERE schedule_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Schedule>(null, $"Не найдено расписание с этим id !");
        return new Success<Schedule>(result);
    }

    public async Task<IOperationResult<Guid>> Put(ScheduleModel schedule)
    {
        var scheduleId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO schedules (schedule_id, psychologist_id, work_day, start_time, end_time,is_booked) VALUES (@scheduleId, @psychologistId, @workDay, @startTime, @endTime, @isBooked)",
            new NpgsqlParameter("@scheduleId", scheduleId),
            new NpgsqlParameter("@psychologistId", schedule.PsychologistId),
            new NpgsqlParameter("@workDay", schedule.WorkDay),
            new NpgsqlParameter("@startTime", schedule.StartTime),
            new NpgsqlParameter("@endTime", schedule.EndTime),
            new NpgsqlParameter("@isBooked", schedule.IsBooked)
        );
        
        return new Success<Guid>(scheduleId);
    }

    public async Task<IOperationResult> Update(Schedule dataToUpdate)
    {
        var scheduleToUpdate = await GetById(dataToUpdate.ScheduleId);
        if (!scheduleToUpdate.Successful)
            return new ElementNotFound("Не найдено расписание с текущим идентификатором");

        await ExecuteSql(
            "UPDATE schedules SET psychologist_id = COALESCE(@psychologistId, psychologist_id), work_day = COALESCE(@workDay, work_day), start_time = COALESCE(@startTime, start_time), end_time = COALESCE(@endTime, end_time),is_booked = COALESCE(@isBooked, is_booked) WHERE schedule_id = @id",
            new NpgsqlParameter("@psychologistId", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@workDay", dataToUpdate.WorkDay),
            new NpgsqlParameter("@startTime", dataToUpdate.StartTime),
            new NpgsqlParameter("@endTime", dataToUpdate.EndTime),
            new NpgsqlParameter("@id", dataToUpdate.ScheduleId),
            new NpgsqlParameter("@isBooked", dataToUpdate.IsBooked));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var scheduleToUpdate = await GetById(id);
        if (!scheduleToUpdate.Successful)
            return new ElementNotFound("Не найдено расписание с текущим идентификатором");

        await ExecuteSql("DELETE FROM schedules WHERE schedule_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}