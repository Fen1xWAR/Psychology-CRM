using CRM.Domain.Models;
using Npgsql;

namespace CRM.Infrastructure.CreationObjectFromSQL;

public class ContactCreator : ICreator<Contact>
{
    public Contact Map(NpgsqlDataReader reader)
    {
        return new Contact
        {
            ContactId = reader.GetGuid(0),
            Email = reader.GetString(1),
            PhoneNumber = reader.GetString(2)
        };
    }
}