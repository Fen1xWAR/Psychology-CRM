using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IClientRepository //интерфейс для репозитория (мало ли поменяем)
{
    Task<IOperationResult<IEnumerable<Client>>> GetAll();
    Task<IOperationResult<Client?>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Client client);
    Task<IOperationResult> Update(Client dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}