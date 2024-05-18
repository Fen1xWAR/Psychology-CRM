using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface IContactRepository
{
    Task<IOperationResult<IEnumerable<Contact>>> Get(int page, int pageSize);
    Task<IOperationResult<IEnumerable<Contact>>> GetAll();
    Task<IOperationResult<Contact>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(ContactModel contact);
    Task<IOperationResult> Update(Contact dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}