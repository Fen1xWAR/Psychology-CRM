using System.Threading.Tasks;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class ServiceRepository : RepositoryBase, IServiceRepository
{
    public ServiceRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Service>> GetAll()
    {
        return await GetDataSql<Service, ServiceCreator>("SELECT * FROM services");
    }

    public async Task<Service> GetById(Guid id)
    {
        return (await GetDataSql<Service, ServiceCreator>("SELECT * FROM services WHERE service_id = @id",
            new NpgsqlParameter("@id", id))).First();
    }

    public async Task Put(Service service)
    {
        var serviceId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO services (service_id, service_name, service_price, service_description) VALUES (@id, @name, @price, @description)",
            new NpgsqlParameter("@id", serviceId),
            new NpgsqlParameter("@name", service.ServiceName),
            new NpgsqlParameter("@price", service.ServicePrice),
            new NpgsqlParameter("@description", service.ServiceDescription));
    }

    public async Task Update(Service dataToUpdate)
    {
        await ExecuteSql(
            "UPDATE services SET service_name = COALESCE(@name, service_name), service_price = COALESCE(@price, service_price), service_description = COALESCE(@description, service_description) WHERE service_id = @id",
            new NpgsqlParameter("@name", dataToUpdate.ServiceName),
            new NpgsqlParameter("@price", dataToUpdate.ServicePrice),
            new NpgsqlParameter("@description", dataToUpdate.ServiceDescription),
            new NpgsqlParameter("@id", dataToUpdate.ServiceId));
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql("DELETE FROM services WHERE service_id = @id", new NpgsqlParameter("@id", id));
    }
}