using CRM.Core.Interfaces;

namespace CRM.Infrastructure.Interfaces;

using CRM.Domain.Models;

public interface IFileRepository
{
    Task<IOperationResult<IEnumerable<File>>> GetAll();
    Task<IOperationResult<File>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(File file);
    Task<IOperationResult> Update(File dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}