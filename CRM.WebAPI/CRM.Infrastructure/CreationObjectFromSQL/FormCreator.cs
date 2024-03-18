using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class FormCreator : ICreator<Form>
{
    public Form Map(NpgsqlDataReader reader)
    {
        return new Form()
        {
            FormId = reader.GetGuid(0),
            FormContent = reader.GetString(1)
        };
    }
}