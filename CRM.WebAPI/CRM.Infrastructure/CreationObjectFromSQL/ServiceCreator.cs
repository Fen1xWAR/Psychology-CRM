using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class ServiceCreator:ICreator<Service>
{
    public Service Map(NpgsqlDataReader reader)
    {
        return new Service()
        {
            ServiceId = reader.GetGuid(0),
            ServiceName = reader.GetString(1),
            ServicePrice = reader.GetDecimal(2),
            ServiceDescription = reader.GetString(3)
        };

    }
}