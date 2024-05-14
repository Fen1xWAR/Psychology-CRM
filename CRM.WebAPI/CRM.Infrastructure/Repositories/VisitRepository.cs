using System.Linq;
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

public class VisitRepository : RepositoryBase, IVisitRepository
{
    public VisitRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Visit>>> GetAll()
    {
        return new Success<IEnumerable<Visit>>(await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits"));
    }

    public async Task<IOperationResult<IEnumerable<Visit>>> GetAllByClientId(Guid id)
    {

        // return new Success<IEnumerable<Visit>>(await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits"));
        return new Success<IEnumerable<Visit>>(
            await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits WHERE client_id=@clientId",
                new NpgsqlParameter("@clientId", id)));
    }
    public async Task<IOperationResult<IEnumerable<Visit>>> GetAllByPsychologistId(Guid id)
    {
        return new Success<IEnumerable<Visit>>(
            await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits WHERE visits.psychologist_id=@psychologistId",
                new NpgsqlParameter("@psychologistId", id)));
    }

    public async Task<IOperationResult<Visit>> GetById(Guid id)
    {
        var result = (await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits WHERE visit_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Visit>(null, $"Not found visit with id {id}");
        return new Success<Visit>(result);
    }

    public async Task<IOperationResult<Guid>> Put(VisitModel visit)
    {
        var visitId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO visits (visit_id, client_id, date_time, psychologist_description, client_note, service_id, psychologist_id) " +
            "VALUES (@id, @clientId, @dateTime, @psychologistDescription, @clientNote, @serviceId, @psychologistId)",
            new NpgsqlParameter("@id", visitId),
            new NpgsqlParameter("@clientId", visit.ClientId),
            new NpgsqlParameter("@dateTime", visit.DateTime),
            new NpgsqlParameter("@psychologistDescription", visit.PsychologistDescription ?? ""),
            new NpgsqlParameter("@clientNote", visit.ClientNote ?? ""),
            new NpgsqlParameter("@serviceId", visit.ServiceId),
            new NpgsqlParameter("@psychologistId", visit.PsychologistId));
        return new Success<Guid>(visitId);
    }

    public async Task<IOperationResult> Update(Visit dataToUpdate)
    {
        var visitToUpdate = await GetById(dataToUpdate.VisitId);
        if (!visitToUpdate.Successful)
            return new ElementNotFound("Not found visit with current id");

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
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var visitToDelete = await GetById(id);
        if (!visitToDelete.Successful)
            return new ElementNotFound("Not found visit with current id");

        await ExecuteSql("DELETE FROM visits WHERE visit_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}