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
            PhoneNumber = reader.GetString(1),
            Name = reader.GetString(2),
            Lastname = reader.GetString(3)
        };
    }
}