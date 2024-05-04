using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface ILoginRepository
{
    Task<IOperationResult<IEnumerable<Login>>> GetAll();
    Task<IOperationResult<Login>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Login login);
    Task<IOperationResult> Update(Login dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}