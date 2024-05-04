using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class LoginRepository : RepositoryBase, ILoginRepository
{
    public LoginRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Login>>> GetAll()
    {
        return new Success<IEnumerable<Login>>(await GetDataSql<Login, LoginCreator>("SELECT * FROM logins"));
    }

    public async Task<IOperationResult<Login>> GetById(Guid id)
    {
        var result = (await GetDataSql<Login, LoginCreator>("SELECT * FROM logins WHERE login_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Login>(null, "LoginNotFound");


        return new Success<Login>(result);
    }

    public async Task<IOperationResult<Guid>> Put(Login login)
    {
        var loginId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO logins (login_id, user_id, login_time, logout_time) VALUES (@id, @userId, @loginTime, @logoutTime)",
            new NpgsqlParameter("@id", loginId),
            new NpgsqlParameter("@userId", login.UserId),
            new NpgsqlParameter("@loginTime", login.LoginTime),
            new NpgsqlParameter("@logoutTime", login.LogoutTime));
        return new Success<Guid>(loginId);
    }

    public async Task<IOperationResult> Update(Login dataToUpdate)
    {
        var loginToUpdate = await GetById(dataToUpdate.LoginId);
        if (!loginToUpdate.Successful)
            return new ElementNotFound("Not found login with current id");
        await ExecuteSql(
            "UPDATE logins SET user_id = COALESCE(@userId, user_id), login_time = COALESCE(@loginTime, login_time), logout_time = COALESCE(@logoutTime, logout_time) WHERE login_id = @id",
            new NpgsqlParameter("@userId", dataToUpdate.UserId),
            new NpgsqlParameter("@loginTime", dataToUpdate.LoginTime),
            new NpgsqlParameter("@logoutTime", dataToUpdate.LogoutTime),
            new NpgsqlParameter("@id", dataToUpdate.LoginId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var loginToDelete = await GetById(id);
        if (!loginToDelete.Successful)
            return new ElementNotFound("Not found login");
        await ExecuteSql("DELETE FROM logins WHERE login_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}