using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IServiceRepository
{
    Task<IOperationResult< IEnumerable<Service>>> GetAll();
    Task<IOperationResult< Service>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Service service);
    Task<IOperationResult> Update(Service service);
    Task<IOperationResult> RemoveById(Guid id);
}