namespace CRM.Domain.Models;

public class Contact
{
    public Guid ContactId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}