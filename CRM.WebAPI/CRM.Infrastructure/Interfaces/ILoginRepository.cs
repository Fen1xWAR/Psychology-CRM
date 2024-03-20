using CRM.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CRM.Infrastructure.Interfaces;

public interface ILoginRepository
{
    Task<IEnumerable<Login>> GetAll();
    Task<Login> GetById(Guid id);
    Task Put(Login login);
    Task Update(Login dataToUpdate);
    Task RemoveById(Guid id);
}