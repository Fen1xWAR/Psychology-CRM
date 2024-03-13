using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class ClientRepository : RepositoryBase, IClientRepository //Репозиторий для работы с таблицей клиентов
{
    public ClientRepository(IConfiguration configuration) : base(configuration) // конфигурацию входа в бд - в класс родителя
    {
    }

    public async Task<IEnumerable<Client>> GetAll()
    {
        return await GetDataSql<Client, ClientCreator>("SELECT * FROM clients"); // вызываем методы родителя (метод getdata для чтения и ExecuteSQL чтобы просто выполнить запрос и не вернуть данные )
    }

    public async Task<Client> GetById(Guid id)
    {
        return (await GetDataSql<Client, ClientCreator>($"SELECT * FROM clients WHERE client_id = '{id}'")).First();
    }

    public async Task Put(Client client)
    {
        var clientId = Guid.NewGuid(); //создаем новый гуидник каждый раз когда добавляем нового клиента
        //выполняем запрос, кладем значения на нужные места
        await ExecuteSql($"INSERT INTO clients (client_id, name, lastname,form, current_problem, contact_id " +
                               $") VALUES ('{clientId}','{client.Name}','{client.Lastname}','{client.Form}','{client.CurrentProblem}','{client.ContactId}')");
    }

    public async Task Update(Client dataToUpdate)
    {// обновляем нужного клиента, если значение новое пришло - ставим его, если нет - оставляем старое
        await ExecuteSql($"UPDATE clients SET name = coalesce('{dataToUpdate.Name}', name), " +
                         $"lastname = coalesce('{dataToUpdate.Lastname}', lastname), " +
                         $"form = coalesce('{dataToUpdate.Form}', form)," +
                         $"current_problem = coalesce('{dataToUpdate.CurrentProblem}', current_problem), " +
                         $"contact_id = coalesce('{dataToUpdate.ContactId}',contact_id) " +
                         $"WHERE client_id = '{dataToUpdate.ClientId}'" );
    }

    public async Task RemoveById(Guid id)
    {
        //удаляем из базы по id
        await ExecuteSql($"DELETE FROM clients WHERE client_id = '{id}'");
    }
}