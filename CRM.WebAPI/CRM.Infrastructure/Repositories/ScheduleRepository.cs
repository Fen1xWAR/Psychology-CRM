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

    public async Task<IOperationResult<Schedule>> GetById(Guid id)
    {
        var result = (await GetDataSql<Schedule, ScheduleCreator>("SELECT * FROM schedules WHERE schedule_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Schedule>(null, $"Not found schedule with id {id}!");
        return new Success<Schedule>(result);
    }

    public async Task<IOperationResult<Guid>> Put(ScheduleModel schedule)
    {
        var scheduleId = Guid.NewGuid();
        var parameters = new List<NpgsqlParameter>
        {
            new NpgsqlParameter("@id", scheduleId),
            new NpgsqlParameter("@psychologistId", schedule.PsychologistId),
            new NpgsqlParameter("@workDay", schedule.WorkDay),
            new NpgsqlParameter("@startTime", schedule.StartTime),
            new NpgsqlParameter("@endTime", schedule.EndTime)
        };

        var sql = "INSERT INTO schedules (schedule_id, psychologist_id, work_day, start_time, end_time";
        var values = "@id, @psychologistId, @workDay, @startTime, @endTime";

        if (schedule.VisitId != null)
        {
            sql += ", visitId";
            values += ", @visitId";
            parameters.Add(new NpgsqlParameter("@visitId", schedule.VisitId));
        }

        sql += ") VALUES (" + values + ")";
        await ExecuteSql(sql, parameters.ToArray());
        return new Success<Guid>(scheduleId);
    }

    public async Task<IOperationResult> Update(Schedule dataToUpdate)
    {
        var scheduleToUpdate = await GetById(dataToUpdate.ScheduleId);
        if (!scheduleToUpdate.Successful)
            return new ElementNotFound("Not found schedule with current id");

        await ExecuteSql(
            "UPDATE schedules SET psychologist_id = COALESCE(@psychologistId, psychologist_id), work_day = COALESCE(@workDay, work_day), start_time = COALESCE(@startTime, start_time), end_time = COALESCE(@endTime, end_time),visitid = COALESCE(@visitId, visitid) WHERE schedule_id = @id",
            new NpgsqlParameter("@psychologistId", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@workDay", dataToUpdate.WorkDay),
            new NpgsqlParameter("@startTime", dataToUpdate.StartTime),
            new NpgsqlParameter("@endTime", dataToUpdate.EndTime),
            new NpgsqlParameter("@id", dataToUpdate.ScheduleId),
            new NpgsqlParameter("@visitId", dataToUpdate.VisitId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var scheduleToUpdate = await GetById(id);
        if (!scheduleToUpdate.Successful)
            return new ElementNotFound("Not found schedule with current id");

        await ExecuteSql("DELETE FROM schedules WHERE schedule_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}