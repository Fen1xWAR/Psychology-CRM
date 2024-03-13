using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class ClientRepository : RepositoryBase, IClientRepository
{
    public ClientRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        return await GetDataSql<Client, ClientCreator>("SELECT * FROM clients");
    }

    public async Task<Client> GetById(Guid id)
    {
        return (await GetDataSql<Client, ClientCreator>($"SELECT * FROM clients WHERE client_id = '{id}'")).First();
    }

    public async Task Put(Client client)
    {
        var clientId = Guid.NewGuid();
        await ExecuteSql($"INSERT INTO clients (client_id, name, lastname,form, current_problem, contact_id " +
                         $") VALUES ('{clientId}','{client.Name}','{client.Lastname}','{client.Form}','{client.CurrentProblem}','{client.ContactId}')");
    }

    public async Task Update(Client dataToUpdate)
    {
        await ExecuteSql($"UPDATE clients SET name = coalesce('{dataToUpdate.Name}', name), " +
                         $"lastname = coalesce('{dataToUpdate.Lastname}', lastname), " +
                         $"form = coalesce('{dataToUpdate.Form}', form)," +
                         $"current_problem = coalesce('{dataToUpdate.CurrentProblem}', current_problem), " +
                         $"contact_id = coalesce('{dataToUpdate.ContactId}',contact_id) " +
                         $"WHERE client_id = '{dataToUpdate.ClientId}'" );
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM clients WHERE client_id = '{id}'");
    }
}