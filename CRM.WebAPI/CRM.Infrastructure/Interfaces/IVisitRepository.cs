using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IVisitRepository
{
    Task<IEnumerable<Visit>> GetAll();
    Task<Visit> GetById(Guid id);
    Task Put(Visit visit);
    Task Update(Visit dataToUpdate);
    Task RemoveById(Guid id);
}