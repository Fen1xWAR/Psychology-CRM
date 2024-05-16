namespace CRM.Domain.ModelsToUpload;

public class UserRegModel
{
    //В таблицу Users
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    
    //В таблицу Contacts
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    
    //В таблицу Clients/Psychologist
    //А ничего сюда не идет KekW
    
    
}