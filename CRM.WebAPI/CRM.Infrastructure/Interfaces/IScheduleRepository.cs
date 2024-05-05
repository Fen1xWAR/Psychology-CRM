using CRM.Core.Interfaces;
using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IScheduleRepository
{
    Task<IOperationResult< IEnumerable<Schedule>>> GetAll();
    Task<IOperationResult< Schedule>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(Schedule schedule);
    Task<IOperationResult> Update(Schedule schedule);
    Task<IOperationResult> RemoveById(Guid id);
}