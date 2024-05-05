namespace CRM.Domain.Models;

public class UserBase
{
    public Guid UserId { get; set; }
    public string Role { get; set; }
}