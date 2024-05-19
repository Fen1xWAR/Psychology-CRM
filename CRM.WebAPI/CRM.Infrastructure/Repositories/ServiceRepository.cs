using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
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

    public async Task<IOperationResult<IEnumerable<Service>>> GetAll()
    {
        return new Success<IEnumerable<Service>>(await GetDataSql<Service, ServiceCreator>("SELECT * FROM services"));
    }

    public async Task<IOperationResult<IEnumerable<Service>>> GetByPsychologistId(Guid id)
    {
        return new Success<IEnumerable<Service>>(await GetDataSql<Service, ServiceCreator>("SELECT * FROM services where psychologist_id=@id  ",
            new NpgsqlParameter("@id",id)));
        
    }


    public async Task<IOperationResult<Service>> GetById(Guid id)
    {
        var result = (await GetDataSql<Service, ServiceCreator>("SELECT * FROM services WHERE service_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Service>(null, "Service not found");
        return new Success<Service>(result);
    }

    public async Task<IOperationResult<Guid>> Put(ServiceModel service)
    {
        var serviceId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO services (service_id, service_name, service_price,psychologist_id) VALUES (@id, @name, @price, @psychologist_id)",
            new NpgsqlParameter("@id", serviceId),
            new NpgsqlParameter("@name", service.ServiceName),
            new NpgsqlParameter("@price", service.ServicePrice),
            new NpgsqlParameter("@description", service.PsychologistId));
        return new Success<Guid>(serviceId);
    }

    public async Task<IOperationResult> Update(Service dataToUpdate)
    {
        var serviceToUpdate = await GetById(dataToUpdate.ServiceId);
        if (!serviceToUpdate.Successful)
            return new ElementNotFound("Not found service with current id");

        await ExecuteSql(
            "UPDATE services SET service_name = COALESCE(@name, service_name), service_price = COALESCE(@price, service_price), psychologist_id = COALESCE(@psychologist_id, psychologist_id) WHERE service_id = @id",
            new NpgsqlParameter("@name", dataToUpdate.ServiceName),
            new NpgsqlParameter("@price", dataToUpdate.ServicePrice),
            new NpgsqlParameter("@description", dataToUpdate.PsychologistId),
            new NpgsqlParameter("@id", dataToUpdate.ServiceId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var serviceToDelete = await GetById(id);
        if (!serviceToDelete.Successful)
            return new ElementNotFound("Not found service with current id");

        await ExecuteSql("DELETE FROM services WHERE service_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}