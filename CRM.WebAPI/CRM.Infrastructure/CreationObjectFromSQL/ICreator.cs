using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public interface ICreator<T> where T: class //интерфейс сборщика с методом собрать
{
    T Map(NpgsqlDataReader reader);
}