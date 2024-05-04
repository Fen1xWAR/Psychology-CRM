using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IFormRepository
{
     Task<IOperationResult<IEnumerable<Form>>> GetAll();
     Task<IOperationResult<Form>> GetById(Guid id);
     Task<IOperationResult<Guid>> Put(Form form);
     Task<IOperationResult> Update(Form dataToUpdate);
     Task<IOperationResult> RemoveById(Guid id);
}