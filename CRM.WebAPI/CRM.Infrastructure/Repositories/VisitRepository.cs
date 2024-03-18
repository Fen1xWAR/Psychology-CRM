using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class VisitRepository : RepositoryBase, IVisitRepository
{
    public async Task<IEnumerable<Visit>> GetAll()
    {
        return await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits");
    }

    public async Task<Visit> GetById(Guid id)
    {
        return (await GetDataSql<Visit, VisitCreator>($"SELECT * FROM visits WHERE visit_id = '{id}'")).First();
    }

    public async Task Put(Visit visit)

    {
        var visitId = Guid.NewGuid();
        await ExecuteSql(
            $"INSERT INTO visits (visit_id, client_id, date_time, psychologist_description, client_note, " +
            $"service_id, psychologist_id ) VALUES ('{visitId}', '{visit.ClientId}', " +
            $"'{visit.DateTime:yyyy-MM-dd HH:mm:ss.fff}', " +
            $"'{visit.ClientNote}', '{visit.PsychologistDescription}', '{visit.ServiceId}', '{visit.PsychologistId}')");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM visits WHERE visit_id = '{id}' ");
    }

    public async Task Update(Visit dataToUpdate)
    {
        await ExecuteSql(
            $"UPDATE public.visits SET " +
            $"client_id = COALESCE('{dataToUpdate.ClientId}', client_id), " +
            $"date_time = COALESCE('{dataToUpdate.DateTime:yyyy-MM-dd HH:mm:ss.fff}', date_time), " +
            $"client_note = COALESCE('{dataToUpdate.ClientNote}', client_note), " +
            $"psychologist_description = COALESCE('{dataToUpdate.PsychologistDescription}', psychologist_description), " +
            $"service_id = COALESCE('{dataToUpdate.ServiceId}', service_id), " +
            $"psychologist_id = COALESCE('{dataToUpdate.PsychologistId}', psychologist_id) " +
            $"WHERE " +
            $"visit_id = '{dataToUpdate.VisitId}'");
    }


    public VisitRepository(IConfiguration configuration) : base(configuration)
    {
    }
}