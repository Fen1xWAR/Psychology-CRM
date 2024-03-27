using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAll();
    Task<Service> GetById(Guid id);
    Task Put(Service service);
    Task Update(Service service);
    Task RemoveById(Guid id);
}