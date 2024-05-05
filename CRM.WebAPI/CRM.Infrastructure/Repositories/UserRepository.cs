using System.Linq;
using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }


    public async Task<IOperationResult<User>> GetUserByEmail(string? email)
    {
        var result = (await GetDataSql<User, UserCreator>("SELECT * FROM users WHERE email = @email",
            new NpgsqlParameter("@email", email))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<User>(null, "User not found");
        return new Success<User>(result);
    }

    public async Task<IOperationResult<IEnumerable<User>>> GetAll()
    {
        return new Success<IEnumerable<User>>(await GetDataSql<User, UserCreator>("SELECT * FROM users"));
    }

    public async Task<IOperationResult<User>> GetById(Guid id)
    {
        var result = (await GetDataSql<User, UserCreator>("SELECT * FROM users WHERE user_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<User>(null, $"User with id {id} not found");
        return new Success<User>(result);
    }

    public async Task<IOperationResult<Guid>> Put(UserModel user)
    {
        var userId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO users (user_id, email, password, role, contact_id) VALUES (@id, @email, @password, @role, @contactId)",
            new NpgsqlParameter("@id", userId),
            new NpgsqlParameter("@email", user.Email),
            new NpgsqlParameter("@password", user.Password),
            new NpgsqlParameter("@role", user.Role),
            new NpgsqlParameter("@contactId", user.ContactId));
        return new Success<Guid>(userId);
    }

    public async Task<IOperationResult> Update(User dataToUpdate)
    {
        var userToUpdate = await GetById(dataToUpdate.UserId);
        if (!userToUpdate.Successful)
            return new ElementNotFound("Not found user with current Id");
        await ExecuteSql(
            "UPDATE users SET email = COALESCE(@email, email), password = COALESCE(@password, password), role = COALESCE(@role, role), contact_id = COALESCE(@contactId, contact_id) WHERE user_id = @id",
            new NpgsqlParameter("@email", dataToUpdate.Email),
            new NpgsqlParameter("@password", dataToUpdate.Password),
            new NpgsqlParameter("@role", dataToUpdate.Role),
            new NpgsqlParameter("@contactId", dataToUpdate.ContactId),
            new NpgsqlParameter("@id", dataToUpdate.UserId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var userToDelete = await GetById(id);
        if (!userToDelete.Successful)
            return new ElementNotFound("Not found user with current id");

        await ExecuteSql("DELETE FROM users WHERE user_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}