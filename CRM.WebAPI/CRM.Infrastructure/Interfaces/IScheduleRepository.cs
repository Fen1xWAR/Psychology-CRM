using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IScheduleRepository
{
    Task<IEnumerable<Schedule>> GetAll();
    Task<Schedule> GetById(Guid id);
    Task Put(Schedule schedule);
    Task Update(Schedule schedule);
    Task RemoveById(Guid id);
}