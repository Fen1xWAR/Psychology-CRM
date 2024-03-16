using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using File = CRM.Domain.Models.File;

namespace CRM.Infrastructure.Repositories;

public class FileRepository : RepositoryBase, IFileRepository
{
    public FileRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<File>> GetAll()
    {
        return await GetDataSql<File, FileCreator>("SELECT * FROM files");
    }

    public async Task<File> GetById(Guid id)
    {
        return (await GetDataSql<File, FileCreator>($"SELECT * FROM files WHERE file_id= '{id}'")).First();
    }

    public async Task Put(File file)
    {
        await ExecuteSql($"INSERT INTO files (file_id, client_id, psychologist_id, file_name, file_content) VALUES ('{file.FileId}','{file.ClientId}','{file.PsychologistId}','{file.FileName}','{file.FileContent}')");
    }

    public async Task Update(File dataToUpdate)
    {

        await ExecuteSql($"UPDATE files SET client_id= COALESCE('{dataToUpdate.ClientId}',client_id), psychologist_id=coalesce('{dataToUpdate.PsychologistId}',psychologist_id), file_name=coalesce('{dataToUpdate.FileName}', file_name), file_content=coalesce('{dataToUpdate.FileContent}',file_content) WHERE file_id='{dataToUpdate.FileId}'" );
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM files WHERE file_id='{id}'");
    }
}