using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace CRM.Infrastructure.Repositories;

public class LoginRepository : RepositoryBase,ILoginRepository
{
    public LoginRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Login>> GetAll()
    {
        return await GetDataSql<Login, LoginCreator>(
            "SELECT * FROM logins ");
    }

    public  async Task<Login> GetById(Guid id)
    {
        return (await GetDataSql<Login, LoginCreator>($"Select * from logins where login_id='{id}'")).First();
    }

    public async Task Put(Login login)
    {
        var loginId = Guid.NewGuid();
        await ExecuteSql($"INSERT INTO  logins(login_id, user_id, login_time, logout_time)" +
                         $" VALUES ('{loginId}','{login.UserId}','{login.LoginTime:yyyy-MM-dd HH:mm:ss.fff}','{login.LogoutTime:yyyy-MM-dd HH:mm:ss.fff}')");
    }

    public async Task Update(Login dataToUpdate)
    {
        await ExecuteSql($"UPDATE logins SET " +
                         $"user_id= coalesce('{dataToUpdate.UserId}', user_id)," +
                         $"login_time=coalesce('{dataToUpdate.LoginTime:yyyy-MM-dd HH:mm:ss.fff'}', login_time)," +
                         $"logout_time=coalesce('{dataToUpdate.LoginTime:yyyy-MM-dd HH:mm:ss.fff'}', logout_time)");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM logins WHERE login_id='{id}'");
    }
}