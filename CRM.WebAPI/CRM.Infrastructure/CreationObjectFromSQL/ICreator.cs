using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public interface ICreator<T> where T: class
{
    T Map(NpgsqlDataReader reader);
}