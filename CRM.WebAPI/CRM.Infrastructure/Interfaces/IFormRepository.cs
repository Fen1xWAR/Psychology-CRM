using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IFormRepository
{
     Task<IEnumerable<Form>> GetAll();
     Task<Form> GetById(Guid id);
     Task Put(Form form);
     Task Update(Form dataToUpdate);
     Task RemoveById(Guid id);
}