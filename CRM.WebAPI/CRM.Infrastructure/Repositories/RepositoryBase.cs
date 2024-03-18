using CRM.Infrastructure.CreationObjectFromSQL;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public abstract class RepositoryBase
{
    private IConfiguration _configuration; // конфигурация (тут пока только строка подключения к базе)
    private readonly string _connectionString; // строка подключения

    protected RepositoryBase(IConfiguration configuration) // инициализируем
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _connectionString =
            configuration.GetConnectionString("PostgreConnectionString"); //идем в кфг файл и берем оттуда подключение
    }

    //тут весело это я спер у Зорина:) метод делает так : он принимает тип возращаемого значения (например Client ) и класс, который способен из строки что вернет
    //база собрать нужный обьект, тобишь для клиента это сборщик клиента и тп...мб сложно согласен
    protected async Task<IEnumerable<T>> GetDataSql<T, TCreator>(string sql)
        where T : class where TCreator : ICreator<T>, new()
    {
        var result = new List<T>(); //список результатов нужного типа
        var creator = new TCreator(); //новый сборщик)
        await using var connection = new NpgsqlConnection(_connectionString); // подключаемся к БД
        await connection.OpenAsync();
        await using
            (var command =
             new NpgsqlCommand(sql,
                 connection)) //выполняем команду (она нам пришла аргументом при вызове метода из конкретного репозитория
        {
            var reader = await command.ExecuteReaderAsync(); //запускаем чтение результата
            while (reader.Read()) //читаем его сколько ответов пришло раз )
            {
                result.Add(creator.Map(reader)); //просим наш сборщик собрать нам обьекты и добавляем в результаты
            }
        }

        await connection.CloseAsync(); //закрываем соединение

        return result;
    }

    protected async Task ExecuteSql(string sql) //тут тоже самое но не ждем результат и сборщик нам не нужен
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        await using var connection = new NpgsqlConnection(this._connectionString);
        await connection.OpenAsync();
        await using (var command = new NpgsqlCommand(sql, connection))
        {
            await command.ExecuteNonQueryAsync(); //исполняем !
        }

        await connection.CloseAsync();
    }
}