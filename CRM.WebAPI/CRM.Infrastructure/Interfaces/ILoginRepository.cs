using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface ILoginRepository
{
    Task<IOperationResult<IEnumerable<Login>>> GetAll();
    Task<IOperationResult<Login>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(LoginModel login);
    Task<IOperationResult> Update(Login dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}