using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class VisitRepository : RepositoryBase, IVisitRepository
{
   

    public async Task<IEnumerable<Visit>> GetAll()
    {
        return await GetDataSql<Visit, VisitCreator>("SELECT * FROM visits");
        // var result = new List<Visit>();
        //
        // using var connection = new NpgsqlConnection(
        //     this._configuration.GetConnectionString("PostgreConnectionString"));
        // connection.Open();
        // await using (var command = new NpgsqlCommand("SELECT * FROM visits", connection))
        // {
        //     var reader = await command.ExecuteReaderAsync();
        //     while (reader.Read())
        //     {
        //         result.Add(new Visit
        //         {
        //             VisitId = reader.GetGuid(0),
        //             // ClientId = reader.GetGuid(1),
        //             // DateTime = reader.GetDateTime(2),
        //             ClientNote = reader.GetString(3),
        //         });
        //     }
        // }
        // await connection.CloseAsync();
        // return result;
    }

    public async Task<Visit> GetById(Guid id)
    {
        return (await GetDataSql<Visit, VisitCreator>($"SELECT * FROM visits WHERE visit_id = '{id}'")).First();
    }
       // using var connection = new NpgsqlConnection(
       //     this._configuration.GetConnectionString("PostgreConnectionString"));
       // await connection.OpenAsync();
       // using (var command = new NpgsqlCommand("SELECT * FROM visits WHERE visit_id = @id", connection))
       // {
       //     command.Parameters.AddWithValue("id", id);
       //     Visit result = null;
       //     var reader = await command.ExecuteReaderAsync();
       //     if (await reader.ReadAsync())
       //     {
       //         result = new Visit
       //         {
       //             VisitId = reader.GetGuid(0),
       //             // ClientId = reader.GetGuid(1),
       //             // DateTime = reader.GetDateTime(2),
       //             ClientNote = reader.GetString(3),
       //         };
       //     }
       //
       //     return result;
       // }
       public async Task Put(Visit visit)
       
       {
           
           var visitId = Guid.NewGuid();
           await ExecuteSQL($"INSERT INTO visits (visit_id, client_id, date_time, client_note, service_id, psychologist_id, visit_note) VALUES ('{visitId}', '{visit.ClientId}', '{visit.DateTime}', '{visit.ClientNote}', '{visit.ServiceID}', '{visit.PsychologistID}', '{visit.VisitNote}')");

       }
    public async Task RemoveById(Guid id)
    {
        await ExecuteSQL($"DELETE FROM visits WHERE visit_id = '{id}' ");
        // using var connection = new NpgsqlConnection(
        //     this._configuration.GetConnectionString("PostgreConnectionString"));
        // await connection.OpenAsync();
        // using (var command = new NpgsqlCommand("DELETE FROM visits WHERE visit_id = @id",connection))
        // {
        //     command.Parameters.AddWithValue("id", id);
        //     await command.ExecuteNonQueryAsync();
        // }

        // await connection.CloseAsync();
    }

    public VisitRepository(IConfiguration configuration) : base(configuration)
    {
    }
}