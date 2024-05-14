using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
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

    public async Task<IOperationResult<Client>> GetByUserId(Guid userId)
    {
        var result = (await GetDataSql<Client, ClientCreator>("SELECT * FROM clients WHERE user_id = @id",
            new NpgsqlParameter("@id", userId))).FirstOrDefault();
        if (result == null)
        {
            return new ElementNotFound<Client>(null, "Client not found");
        }

        return new Success<Client>(result);
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

    public async Task<IOperationResult<Guid>> Put(ClientModel client)
    {
        var clientId = Guid.NewGuid();
           await ExecuteSql(
            "INSERT INTO clients (client_id, form_id, current_problem, user_id) VALUES (@clientId, @formId, @currentProblem, @userId)",
            new NpgsqlParameter("@clientId", clientId),
            new NpgsqlParameter("@formId", client.FormId),
            new NpgsqlParameter("@currentProblem", client.CurrentProblem),
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
            "UPDATE clients SET form_id = COALESCE(@form, form_id), current_problem = COALESCE(@currentProblem, current_problem),  user_id = COALESCE(@userId, user_id) WHERE client_id = @id",
            new NpgsqlParameter("@form", dataToUpdate.FormId),
            new NpgsqlParameter("@currentProblem", dataToUpdate.CurrentProblem),
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