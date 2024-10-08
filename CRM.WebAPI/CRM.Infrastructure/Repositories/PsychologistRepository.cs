
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

    public async  Task<IOperationResult<IEnumerable<Psychologist>>> Get(int page, int pageSize)
    {
        var offset = (page - 1) * pageSize ;
        offset = offset >= 0 ? offset : 0;
        var query = $"SELECT * FROM psychologists OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
        return new Success<IEnumerable<Psychologist>>(await GetDataSql<Psychologist, PsychologistCreator>(query));
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
            return new ElementNotFound<Psychologist>(null, $"Не найден психолог с этим  идентификатором ");
        return new Success<Psychologist>(result);
    }

    public async Task<IOperationResult<Psychologist>> GetByUserId(Guid userId)
    {
        var result =  (await GetDataSql<Psychologist, PsychologistCreator>(
            "SELECT * FROM psychologists WHERE user_id = @id",
            new NpgsqlParameter("@id", userId))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Psychologist>(null, $"Не найдет психолог с этим  пользовательским  идентификатором ");
        return new Success<Psychologist>(result);
    }

    public async Task<IOperationResult<Guid>> Put(PsychologistModel psychologist)
    {
        var psychologistId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO psychologists (psychologist_id, user_id,about) VALUES (@id, @userId,@about)",
            new NpgsqlParameter("@id", psychologistId),
            new NpgsqlParameter("@userId", psychologist.UserId),
            new NpgsqlParameter("@about", psychologist.About));
        return new Success<Guid>(psychologistId);
    }

    public async Task<IOperationResult> Update(Psychologist dataToUpdate)
    {
        var psychologistToUpdate = await GetById(dataToUpdate.PsychologistId);
        if (!psychologistToUpdate.Successful)
            return new ElementNotFound("Не найден психолог с текушим идентификатором");
        
        await ExecuteSql(
            "UPDATE psychologists SET user_id = COALESCE(@userId, user_id),about = COALESCE(@about, about) WHERE psychologist_id = @id",
            new NpgsqlParameter("@userId", dataToUpdate.UserId),
            new NpgsqlParameter("@id", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@about", dataToUpdate.About));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var psychologistToDelete = await GetById(id);
        if (!psychologistToDelete.Successful)
            return new ElementNotFound("Не найден психолог с текушим идентификатором");

        await ExecuteSql("DELETE FROM psychologists WHERE psychologist_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}