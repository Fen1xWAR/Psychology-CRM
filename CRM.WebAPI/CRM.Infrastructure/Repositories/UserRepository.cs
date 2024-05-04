using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<User?> CreateUser(UserAuth model)
    {
        // TODO: Implement user creation logic
        return null;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return (await GetDataSql<User, UserCreator>("SELECT * FROM users WHERE email = @email",
            new NpgsqlParameter("@email", email))).FirstOrDefault();
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await GetDataSql<User, UserCreator>("SELECT * FROM users");
    }

    public async Task<User?> GetById(Guid id)
    {
        return (await GetDataSql<User, UserCreator>("SELECT * FROM users WHERE user_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
    }

    public async Task Put(User user)
    {
        var userId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO users (user_id, email, password, role) VALUES (@id, @email, @password, @role)",
            new NpgsqlParameter("@id", userId),
            new NpgsqlParameter("@email", user.Email),
            new NpgsqlParameter("@password", user.Password),
            new NpgsqlParameter("@role", user.Role));
    }

    public async Task Update(User dataToUpdate)
    {
        await ExecuteSql(
            "UPDATE users SET email = COALESCE(@email, email), password = COALESCE(@password, password), role = COALESCE(@role, role) WHERE user_id = @id",
            new NpgsqlParameter("@email", dataToUpdate.Email),
            new NpgsqlParameter("@password", dataToUpdate.Password),
            new NpgsqlParameter("@role", dataToUpdate.Role),
            new NpgsqlParameter("@id", dataToUpdate.UserId));
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql("DELETE FROM users WHERE user_id = @id", new NpgsqlParameter("@id", id));
    }
}