using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface IServiceRepository
{
    Task<IOperationResult< IEnumerable<Service>>> GetAll();
    Task<IOperationResult<IEnumerable<Service>>> GetByPsychologistId(Guid id);
    Task<IOperationResult< Service>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(ServiceModel service);
    Task<IOperationResult> Update(Service service);
    Task<IOperationResult> RemoveById(Guid id);
}