using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class ServiceRepository:RepositoryBase,IServiceRepository
{
    public ServiceRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Service>> GetAll()
    {
        return
            await GetDataSql<Service, ServiceCreator>(
                "SELECT * FROM services"); 
    }

    public async Task<Service> GetById(Guid id)
    {
        return (await GetDataSql<Service, ServiceCreator>
            ($"SELECT * FROM services WHERE service_id = '{id}'")).First();
    }

    public async Task Put(Service service)
    {
        var serviceId = Guid.NewGuid(); 
        await ExecuteSql($"INSERT INTO services (service_id, service_name, service_price, service_description " +
                         $") VALUES ('{serviceId}','{service.ServiceName}','{service.ServicePrice}','{service.ServiceDescription}')");
    }

    public async Task Update(Service dataToUpdate)
    {
        await ExecuteSql($"UPDATE services SET " +
                         $"service_name = coalesce('{dataToUpdate.ServiceName}', service_name), " +
                         $"service_price = coalesce('{dataToUpdate.ServicePrice}', service_price), " +
                         $"service_description = coalesce('{dataToUpdate.ServiceDescription}',service_description) " +
                         $"WHERE service_id = '{dataToUpdate.ServiceId}'");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM services WHERE service_id = '{id}'");
    }
}