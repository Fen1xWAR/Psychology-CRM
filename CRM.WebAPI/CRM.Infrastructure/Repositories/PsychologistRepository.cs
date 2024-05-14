
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
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

    public async Task<IOperationResult<IEnumerable<Psychologist>>> GetAll()
    {
        return new Success<IEnumerable<Psychologist>>( await GetDataSql<Psychologist, PsychologistCreator>("SELECT * FROM psychologists"));
    }

    public async Task<IOperationResult< Psychologist>> GetById(Guid id)
    {
        
        var result =  (await GetDataSql<Psychologist, PsychologistCreator>(
            "SELECT * FROM psychologists WHERE psychologist_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Psychologist>(null, $"Not found psychologist with id {id}");
        return new Success<Psychologist>(result);
    }

    public async Task<IOperationResult<Psychologist>> GetByUserId(Guid userId)
    {
        var result =  (await GetDataSql<Psychologist, PsychologistCreator>(
            "SELECT * FROM psychologists WHERE user_id = @id",
            new NpgsqlParameter("@id", userId))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Psychologist>(null, $"Not found psychologist with user id {userId}");
        return new Success<Psychologist>(result);
    }

    public async Task<IOperationResult<Guid>> Put(PsychologistModel psychologist)
    {
        var psychologistId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO psychologists (psychologist_id, user_id) VALUES (@id, @userId)",
            new NpgsqlParameter("@id", psychologistId),
            new NpgsqlParameter("@userId", psychologist.UserId));
        return new Success<Guid>(psychologistId);
    }

    public async Task<IOperationResult> Update(Psychologist dataToUpdate)
    {
        var psychologistToUpdate = await GetById(dataToUpdate.PsychologistId);
        if (!psychologistToUpdate.Successful)
            return new ElementNotFound("Not found psychologist with current id");
        
        await ExecuteSql(
            "UPDATE psychologists SET user_id = COALESCE(@userId, user_id) WHERE psychologist_id = @id",
            new NpgsqlParameter("@userId", dataToUpdate.UserId),
            new NpgsqlParameter("@id", dataToUpdate.PsychologistId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var psychologistToDelete = await GetById(id);
        if (!psychologistToDelete.Successful)
            return new ElementNotFound("Not found psychologist with current id");

        await ExecuteSql("DELETE FROM psychologists WHERE psychologist_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}