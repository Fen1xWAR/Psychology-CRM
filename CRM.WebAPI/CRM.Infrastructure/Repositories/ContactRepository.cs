using CRM.Core.Implement;
using CRM.Core.Interfaces;
using CRM.Domain.Models;
using CRM.Domain.ModelsToUpload;
using CRM.Infrastructure.CreationObjectFromSQL;
using CRM.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CRM.Infrastructure.Repositories;

public class ContactRepository : RepositoryBase, IContactRepository
{
    public ContactRepository(IConfiguration configuration) : base(configuration)
    {
    }
    
    public async Task<IOperationResult<IEnumerable<Contact>>> GetAll()
    {
        return new Success<IEnumerable<Contact>>(await GetDataSql<Contact, ContactCreator>("SELECT * FROM contacts"));
    }

    public async Task<IOperationResult<Contact>> GetById(Guid id)
    {
        var result = (await GetDataSql<Contact, ContactCreator>("SELECT * FROM contacts WHERE contact_id = @id",
            new NpgsqlParameter("@id", id))).FirstOrDefault();
        if (result == null)
            return new ElementNotFound<Contact>(null, "ContactNotFound");


        return new Success<Contact>(result);
    }
    

    public async Task<IOperationResult<Guid>> Put(ContactModel contact)
    {
        var contactId = Guid.NewGuid();
        await ExecuteSql(
            "INSERT INTO contacts (contact_id, phone_number, name, lastname,middlename,date_of_birth) VALUES (@contactId, @phoneNumber, @name, @lastname, @middlename, @date_of_birth)",
            new NpgsqlParameter("@contactId", contactId),
            new NpgsqlParameter("@phoneNumber", contact.PhoneNumber),
            new NpgsqlParameter("@name", contact.Name),
            new NpgsqlParameter("@lastname", contact.Lastname),
            new NpgsqlParameter("@middlename",contact.Middlename ),
            new NpgsqlParameter("@date_of_birth",contact.DateOfBirth)
            );
        return new Success<Guid>(contactId);
    }

    public async Task<IOperationResult> Update(Contact dataToUpdate)
    {
        var contactToUpdate = await GetById(dataToUpdate.ContactId);
        if (!contactToUpdate.Successful)
            return new ElementNotFound("Not found contact with current Id");
        await ExecuteSql(
            "UPDATE contacts SET phone_number = COALESCE(@phoneNumber, phone_number), name = COALESCE(@name, name), lastname = COALESCE(@lastname, lastname),  middlename = COALESCE(@middlename,  middlename), date_of_birth = COALESCE(@date_of_birth,  date_of_birth) WHERE contact_id = @id",
            new NpgsqlParameter("@phoneNumber", dataToUpdate.PhoneNumber),
            new NpgsqlParameter("@name", dataToUpdate.Name),
            new NpgsqlParameter("@lastname", dataToUpdate.Lastname),
            new NpgsqlParameter("@middlename",dataToUpdate.Middlename ),
            new NpgsqlParameter("@date_of_birth",dataToUpdate.DateOfBirth),
            new NpgsqlParameter("@id", dataToUpdate.ContactId));
        return new Success();
    }

    public async Task<IOperationResult> RemoveById(Guid id)
    {
        var contactToDelete = await GetById(id);
        if (!contactToDelete.Successful)
            return new ElementNotFound("Not found contact with current Id");
        await ExecuteSql("DELETE FROM contacts WHERE contact_id = @id", new NpgsqlParameter("@id", id));
        return new Success();
    }
}