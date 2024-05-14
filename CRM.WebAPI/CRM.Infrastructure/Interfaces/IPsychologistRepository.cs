using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface IPsychologistRepository
{
    Task<IOperationResult< IEnumerable<Psychologist>>> GetAll();
    Task<IOperationResult< Psychologist>> GetById(Guid id);
    Task<IOperationResult<Psychologist>> GetByUserId(Guid userId);
    Task<IOperationResult<Guid>> Put(PsychologistModel psychologist);
    Task<IOperationResult> Update(Psychologist dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}