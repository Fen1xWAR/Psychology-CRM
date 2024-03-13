using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IClientRepository //интерфейс для репозитория (мало ли поменяем)
{
    Task<IEnumerable<Client>> GetAll();
    Task<Client> GetById(Guid id);
    Task Put(Client client);
    Task Update(Client dataToUpdate);
    Task RemoveById(Guid id);
}