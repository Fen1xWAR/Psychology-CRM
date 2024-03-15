namespace CRM.Infrastructure.Interfaces;
using CRM.Domain.Models;
public interface IFileRepository
{
    Task<IEnumerable<File>> GetAll();
    Task<File> GetById(Guid id);
    Task Put(File file);
    Task Update(File dataToUpdate);
    Task RemoveById(Guid id); 
}