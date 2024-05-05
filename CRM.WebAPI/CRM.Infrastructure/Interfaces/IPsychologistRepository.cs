using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IPsychologistRepository
{
    Task<IOperationResult< IEnumerable<Psychologist>>> GetAll();
    Task<IOperationResult< Psychologist>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Psychologist psychologist);
    Task<IOperationResult> Update(Psychologist dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}