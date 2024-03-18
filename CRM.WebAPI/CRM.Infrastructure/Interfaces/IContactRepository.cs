using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAll();
    Task<Contact> GetById(Guid id);
    Task Put(Contact contact);
    Task Update(Contact dataToUpdate);
    Task RemoveById(Guid id);
}