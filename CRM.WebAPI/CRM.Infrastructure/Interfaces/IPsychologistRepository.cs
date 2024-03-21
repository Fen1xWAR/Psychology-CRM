using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IPsychologistRepository
{
    Task<IEnumerable<Psychologist>> GetAll();
    Task<Psychologist> GetById(Guid id);
    Task Put(Psychologist psychologist);
    Task Update(Psychologist dataToUpdate);
    Task RemoveById(Guid id);
}