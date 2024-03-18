using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class FormRepository : RepositoryBase, IFormRepository
{
    public FormRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Form>> GetAll()
    {
        return await GetDataSql<Form, FormCreator>("SELECT * FROM forms");
    }

    public async Task<Form> GetById(Guid id)
    {
        return (await GetDataSql<Form, FormCreator>($"SELECT * FROM forms where form_id='{id}'")).First();
    }

    public async Task Put(Form form)
    {
        var id = Guid.NewGuid();
        await ExecuteSql($"INSERT INTO forms (form_id, form_content) VALUES ('{id}','{form.FormContent}')");
    }

    public async Task Update(Form dataToUpdate)
    {
        await ExecuteSql($"UPDATE forms SET form_content=coalesce(form_content,'{dataToUpdate.FormContent}')  WHERE form_id='{dataToUpdate.FormId}'");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM forms WHERE form_id='{id}'");
    }
}