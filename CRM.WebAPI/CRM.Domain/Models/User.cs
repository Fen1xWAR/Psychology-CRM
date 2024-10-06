namespace CRM.Domain.Models;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public Guid ContactId { get; set; }
}