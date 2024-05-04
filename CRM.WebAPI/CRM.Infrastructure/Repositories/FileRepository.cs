using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using File = CRM.Domain.Models.File;

namespace CRM.Infrastructure.Repositories;

public class FileRepository : RepositoryBase, IFileRepository
{
    public FileRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<File>>> GetAll()
    {
        return new Success<IEnumerable<File>>(await GetDataSql<File, FileCreator>("SELECT * FROM files"));
    }

    public async Task<IOperationResult<File>> GetById(Guid id)
    {
        var result = (await GetDataSql<File, FileCreator>("SELECT * FROM files WHERE file_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
        {
            return new ElementNotFound<File>(null, "File not found");
        }

        return new Success<File>(result);
    }

    public async Task<IOperationResult<Guid>> Put(File file)
    {
        var id = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO files (file_id, client_id, psychologist_id, file_name, file_content) VALUES (@id, @clientId, @psychologistId, @fileName, @fileContent)",
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@clientId", file.ClientId),
            new NpgsqlParameter("@psychologistId", file.PsychologistId),
            new NpgsqlParameter("@fileName", file.FileName),
            new NpgsqlParameter("@fileContent", file.FileContent));
        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> Update(File dataToUpdate)
    {
        var fileToUpdate = await GetById(dataToUpdate.FileId);
        if (!fileToUpdate.Successful)
            return new ElementNotFound("Not found file with current Id");
        await ExecuteSql(
            "UPDATE files SET client_id = COALESCE(@clientId, client_id), psychologist_id = COALESCE(@psychologistId, psychologist_id), file_name = COALESCE(@fileName, file_name), file_content = COALESCE(@fileContent, file_content) WHERE file_id = @id",
            new NpgsqlParameter("@clientId", dataToUpdate.ClientId),
            new NpgsqlParameter("@psychologistId", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@fileName", dataToUpdate.FileName),
            new NpgsqlParameter("@fileContent", dataToUpdate.FileContent),
            new NpgsqlParameter("@id", dataToUpdate.FileId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var fileToDelete = await GetById(id);
        if (!fileToDelete.Successful)

            return new ElementNotFound("Not found file with current Id");

        await ExecuteSql("DELETE FROM files WHERE file_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}