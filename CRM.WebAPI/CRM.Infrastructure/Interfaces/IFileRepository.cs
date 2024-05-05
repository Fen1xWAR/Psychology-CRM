using CRM.Core.Interfaces;
using CRM.Domain.Models;
using File = CRM.Domain.Models.File;


namespace CRM.Infrastructure.Interfaces;
public interface IFileRepository
{
    Task<IOperationResult<IEnumerable<File>>> GetAll();
    Task<IOperationResult<File>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(File file);
    Task<IOperationResult> Update(File dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}