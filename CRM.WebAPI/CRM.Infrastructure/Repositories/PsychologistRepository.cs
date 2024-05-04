using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class PsychologistRepository : RepositoryBase, IPsychologistRepository
{
    public PsychologistRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Psychologist>> GetAll()
    {
        return await GetDataSql<Psychologist, PsychologistCreator>("SELECT * FROM psychologists");
    }

    public async Task<Psychologist> GetById(Guid id)
    {
        return (await GetDataSql<Psychologist, PsychologistCreator>(
            "SELECT * FROM psychologists WHERE psychologist_id = @id",
            new NpgsqlParameter("@id", id))).First();
    }

    public async Task Put(Psychologist psychologist)
    {
        var psychologistId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO psychologists (psychologist_id, contact_id) VALUES (@id, @contactId)",
            new NpgsqlParameter("@id", psychologistId),
            new NpgsqlParameter("@contactId", psychologist.ContactId));
    }

    public async Task Update(Psychologist dataToUpdate)
    {
        await ExecuteSql(
            "UPDATE psychologists SET contact_id = COALESCE(@contactId, contact_id) WHERE psychologist_id = @id",
            new NpgsqlParameter("@contactId", dataToUpdate.ContactId),
            new NpgsqlParameter("@id", dataToUpdate.PsychologistId));
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql("DELETE FROM psychologists WHERE psychologist_id = @id", new NpgsqlParameter("@id", id));
    }
}