using CRM.Infrastructure.CreationObjectFromSQL;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class RepositoryBase
{
    IConfiguration _configuration;
    private string _connectionString;

    protected RepositoryBase(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString = configuration.GetConnectionString("PostgreConnectionString");
    }

    protected async Task<IEnumerable<T>> GetDataSql<T,TCreator>(string sql) where T : class where TCreator : ICreator<T>,new()
    {
        List<T> result = new List<T>();
        var creator = new TCreator();
        using (var connection = new NpgsqlConnection(this._connectionString))
        {
            await connection.OpenAsync();
            await using (var command = new NpgsqlCommand(sql, connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    result.Add(creator.Map(reader));
                }
            }
        
            await connection.CloseAsync();
        }

        return result;
    }
    protected async Task ExecuteSQL(string sql)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        using (var connection = new NpgsqlConnection(this._connectionString))
        {
           await  connection.OpenAsync();
            using (var command = new NpgsqlCommand(sql,connection))
            {
                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();
        }
    }
    
}