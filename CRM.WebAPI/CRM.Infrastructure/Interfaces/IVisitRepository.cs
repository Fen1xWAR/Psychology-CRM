using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface IVisitRepository
{
    Task<IOperationResult<IEnumerable<Visit>>> GetAll();
    Task<IOperationResult<IEnumerable<Visit>>> GetAllByClientId(Guid id);
    Task<IOperationResult<IEnumerable<Visit>>> GetAllByPsychologistId(Guid id);
    Task<IOperationResult<Visit>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(VisitModel visit);
    Task<IOperationResult> Update(Visit dataToUpdate);
    Task<IOperationResult> RemoveById(Guid id);
}