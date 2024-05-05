using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IVisitRepository
{
    Task<IOperationResult<IEnumerable<Visit>>> GetAll();
    Task<IOperationResult<Visit>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Visit visit);
    Task<IOperationResult> Update(Visit dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}