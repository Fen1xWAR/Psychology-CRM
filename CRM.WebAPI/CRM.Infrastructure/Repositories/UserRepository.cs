using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class UserRepository:RepositoryBase,IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return
            await GetDataSql<User, UserCreator>(
                "SELECT * FROM users"); 
    }

    public async Task<User> GetById(Guid id)
    {
        return (await GetDataSql<User, UserCreator>
            ($"SELECT * FROM users WHERE user_id = '{id}'")).First();
    }

    public async Task Put(User user)
    {
        var userId = Guid.NewGuid(); 
        await ExecuteSql($"INSERT INTO users (user_id, username, password, role, data_id" +
                         $") VALUES ('{userId}','{user.UserName}','{user.Password}','{user.Role}','{user.DataId}')");
    }


    public async Task Update(User dataToUpdate)
    {
        await ExecuteSql($"UPDATE users SET username = coalesce('{dataToUpdate.UserName}', username), " +
                         $"password = coalesce('{dataToUpdate.Password}', password), " +
                         $"role = coalesce('{dataToUpdate.Role}',role)," +
                         $"data_id = coalesce('{dataToUpdate.DataId}',data_id)" +
                         $"WHERE user_id = '{dataToUpdate.UserId}'");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM users WHERE user_id = '{id}'");
    }
}