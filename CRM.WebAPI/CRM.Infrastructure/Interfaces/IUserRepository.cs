using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<IOperationResult<User>> GetUserByEmail(string? email);

  
    Task<IOperationResult< IEnumerable<User>>> GetAll();
    Task<IOperationResult< User>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(User user);
    Task<IOperationResult> Update(User dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id); 
}