using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class PsychologistRepository:RepositoryBase,IPsychologistRepository
{
    public PsychologistRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Psychologist>> GetAll()
    {
        return
            await GetDataSql<Psychologist, PsychologistCreator>(
                "SELECT * FROM psychologists"); 
    }

    public async Task<Psychologist> GetById(Guid id)
    {
        return (await GetDataSql<Psychologist, PsychologistCreator>
            ($"SELECT * FROM psychologists WHERE psychologist_id = '{id}'")).First();
    }

    public async Task Put(Psychologist psychologist)
    {
        var psychologistId = Guid.NewGuid(); 
        await ExecuteSql($"INSERT INTO psychologists (psychologist_id, name, lastname, contact_id " +
                         $") VALUES ('{psychologistId}','{psychologist.Name}','{psychologist.Lastname}','{psychologist.ContactId}')");
    }

    public async Task Update(Psychologist dataToUpdate)
    {
        await ExecuteSql($"UPDATE psychologists SET name = coalesce('{dataToUpdate.Name}', name), " +
                         $"lastname = coalesce('{dataToUpdate.Lastname}', lastname), " +
                         $"contact_id = coalesce('{dataToUpdate.ContactId}',contact_id) " +
                         $"WHERE psychologist_id = '{dataToUpdate.PsychologistId}'");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM psychologists WHERE psychologist_id = '{id}'");
    }
}
