using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class ClientRepository : RepositoryBase, IClientRepository
{
    public ClientRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Client>>> GetAll()
    {
        return new Success<IEnumerable<Client>>(await GetDataSql<Client, ClientCreator>("SELECT * FROM clients"));
    }

    public async Task<IOperationResult<Client?>> GetById(Guid id)
    {
        var result = (await GetDataSql<Client, ClientCreator>("SELECT * FROM clients WHERE client_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
        {
            return new ElementNotFound<Client>(null, "Client not found");
        }

        return new Success<Client?>(result);
    }

    public async Task<IOperationResult<Guid>> Put(Client client)
    {
        var clientId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO clients (client_id, form, current_problem, contact_id, user_id) VALUES (@clientId, @form, @currentProblem, @contactId, @userId)",
            new NpgsqlParameter("@clientId", clientId),
            new NpgsqlParameter("@form", client.Form),
            new NpgsqlParameter("@currentProblem", client.CurrentProblem ?? ""),
            new NpgsqlParameter("@contactId", client.ContactId),
            new NpgsqlParameter("@userId", client.UserId));
        return new Success<Guid>(clientId);
    }

    public async Task<IOperationResult> Update(Client dataToUpdate)
    {
        var clientToUpdate = await GetById(dataToUpdate.ClientId);
        if (!clientToUpdate.Successful)
        {
            return new ElementNotFound("Not found client with current Id");
        }

        await ExecuteSql(
            "UPDATE clients SET form = COALESCE(@form, form), current_problem = COALESCE(@currentProblem, current_problem), contact_id = COALESCE(@contactId, contact_id), user_id = COALESCE(@userId, user_id) WHERE client_id = @id",
            new NpgsqlParameter("@form", dataToUpdate.Form),
            new NpgsqlParameter("@currentProblem", dataToUpdate.CurrentProblem),
            new NpgsqlParameter("@contactId", dataToUpdate.ContactId),
            new NpgsqlParameter("@id", dataToUpdate.ClientId),
            new NpgsqlParameter("@userId", dataToUpdate.UserId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var clientToDelete = await GetById(id);
        if (!clientToDelete.Successful)
        {
            return new ElementNotFound("Not found client with current Id");
        }

        await ExecuteSql("DELETE FROM clients WHERE client_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}