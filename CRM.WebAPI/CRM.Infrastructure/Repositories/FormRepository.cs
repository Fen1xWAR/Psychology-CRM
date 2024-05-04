using System.Threading.Tasks;
using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class FormRepository : RepositoryBase, IFormRepository
{
    public FormRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IOperationResult<IEnumerable<Form>>> GetAll()
    {
        return new Success<IEnumerable<Form>>(await GetDataSql<Form, FormCreator>("SELECT * FROM forms"));
    }

    public async Task<IOperationResult<Form>> GetById(Guid id)
    {
        var result = (await GetDataSql<Form, FormCreator>("SELECT * FROM forms WHERE form_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Form>(null, "Form not found ");
        return new Success<Form>(result);
    }

    public async Task<IOperationResult<Guid>> Put(Form form)
    {
        var id = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO forms (form_id, form_content) VALUES (@id, @formContent)",
            new NpgsqlParameter("@id", id),
            new NpgsqlParameter("@formContent", form.FormContent));
        return new Success<Guid>(id);
    }

    public async Task<IOperationResult> Update(Form dataToUpdate)
    {
        var formToUpdate = await GetById(dataToUpdate.FormId);
        if (!formToUpdate.Successful)
            return new ElementNotFound("Not found form with current id!");

        await ExecuteSql(
            "UPDATE forms SET form_content = COALESCE(@formContent,form_content) WHERE form_id = @id",
            new NpgsqlParameter("@formContent", dataToUpdate.FormContent),
            new NpgsqlParameter("@id", dataToUpdate.FormId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var formToDelete = await GetById(id);
        if (!formToDelete.Successful)
            return new ElementNotFound("Not found form with current id!");
        await ExecuteSql("DELETE FROM forms WHERE form_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}