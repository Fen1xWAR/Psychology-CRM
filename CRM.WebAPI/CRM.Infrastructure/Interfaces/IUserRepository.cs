using CRM.Domain.Models;

namespace CRM.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);

    Task<User?> CreateUser(UserAuth model);
    Task<IEnumerable<User>> GetAll();
    Task<User?> GetById(Guid id);
    Task Put(User user);
    Task Update(User dataToUpdate);
    Task RemoveById(Guid id); 
}