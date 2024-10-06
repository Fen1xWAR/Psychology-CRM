using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;

namespace CRM.Infrastructure.Interfaces;

public interface IScheduleRepository
{
    Task<IOperationResult< IEnumerable<Schedule>>> GetAll();

    Task<IOperationResult<IEnumerable<Schedule>>> GetByPsychologistIdAndDay(Guid psychologistId, DateOnly day);
    Task<IOperationResult< Schedule>> GetById(Guid id);
    Task<IOperationResult<Guid>> Put(ScheduleModel schedule);
    Task<IOperationResult> Update(Schedule schedule);
    Task<IOperationResult> RemoveById(Guid id);
}