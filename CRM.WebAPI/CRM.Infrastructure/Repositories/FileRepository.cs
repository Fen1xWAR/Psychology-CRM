using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using File = CRM.Domain.Models.File;

namespace CRM.Infrastructure.Repositories;

public class FileRepository : RepositoryBase, IFileRepository
{
    public FileRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public Task<IEnumerable<File>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<File> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Put(File file)
    {
        throw new NotImplementedException();
    }

    public Task Update(File dataToUpdate)
    {
        throw new NotImplementedException();
    }

    public Task RemoveById(Guid id)
    {
        throw new NotImplementedException();
    }
}