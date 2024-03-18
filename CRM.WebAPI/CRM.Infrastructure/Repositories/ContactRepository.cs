using CRM.Domain.Models;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CRM.Infrastructure.Repositories;

public class ContactRepository : RepositoryBase, IContactRepository
{
    public ContactRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IEnumerable<Contact>> GetAll()
    {
        return await GetDataSql<Contact, ContactCreator>("SELECT * FROM contacts");
    }

    public async Task<Contact> GetById(Guid id)
    {
        return (await GetDataSql<Contact, ContactCreator>($"SELECT * FROM contacts WHERE contact_id='{id}'")).First();
    }

    public async Task Put(Contact contact)
    {
        var contactId = Guid.NewGuid();
        await ExecuteSql(
            $"INSERT INTO contacts (contact_id, email, phone_number) VALUES ('{contactId}','{contact.Email}','{contact.PhoneNumber}') ");
    }

    public async Task Update(Contact dataToUpdate)
    {
        await ExecuteSql(
            $"UPDATE contacts  SET email = coalesce('{dataToUpdate.Email}',email), phone_number = coalesce('{dataToUpdate.PhoneNumber}', phone_number) WHERE contact_id='{dataToUpdate.ContactId}'");
    }

    public async Task RemoveById(Guid id)
    {
        await ExecuteSql($"DELETE FROM contacts WHERE contact_id = '{id}'");
    }
}