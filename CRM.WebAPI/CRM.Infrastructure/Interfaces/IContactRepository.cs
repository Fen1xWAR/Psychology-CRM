using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IContactRepository
{
    Task<IOperationResult<IEnumerable<Contact>>> GetAll();
    Task<IOperationResult<Contact>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Contact contact);
    Task<IOperationResult> Update(Contact dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}