using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class VisitRepository : RepositoryBase, IVisitRepository
{
    public VisitRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Visit>> GetAll()
    {
        return await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits");
    }

    public async Task<Visit> GetById(Guid id)
    {
        return (await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits WHERE visit_id = @id",
            new NpgsqlParameter("@id", id))).First();
    }

    public async Task Put(Visit visit)
    {
        var visitId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO visits (visit_id, client_id, date_time, psychologist_description, client_note, service_id, psychologist_id) " +
            "VALUES (@id, @clientId, @dateTime, @psychologistDescription, @clientNote, @serviceId, @psychologistId)",
            new NpgsqlParameter("@id", visitId),
            new NpgsqlParameter("@clientId", visit.ClientId),
            new NpgsqlParameter("@dateTime", visit.DateTime),
            new NpgsqlParameter("@psychologistDescription", visit.PsychologistDescription),
            new NpgsqlParameter("@clientNote", visit.ClientNote),
            new NpgsqlParameter("@serviceId", visit.ServiceId),
            new NpgsqlParameter("@psychologistId", visit.PsychologistId));
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql("DELETE FROM visits WHERE visit_id = @id", new NpgsqlParameter("@id", id));
    }

    public async Task Update(Visit dataToUpdate)
    {
        await ExecuteSql(
            "UPDATE visits SET client_id = COALESCE(@clientId, client_id), date_time = COALESCE(@dateTime, date_time), " +
            "client_note = COALESCE(@clientNote, client_note), psychologist_description = COALESCE(@psychologistDescription, psychologist_description), " +
            "service_id = COALESCE(@serviceId, service_id), psychologist_id = COALESCE(@psychologistId, psychologist_id) " +
            "WHERE visit_id = @id",
            new NpgsqlParameter("@clientId", dataToUpdate.ClientId),
            new NpgsqlParameter("@dateTime", dataToUpdate.DateTime),
            new NpgsqlParameter("@clientNote", dataToUpdate.ClientNote),
            new NpgsqlParameter("@psychologistDescription", dataToUpdate.PsychologistDescription),
            new NpgsqlParameter("@serviceId", dataToUpdate.ServiceId),
            new NpgsqlParameter("@psychologistId", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@id", dataToUpdate.VisitId));
    }
}