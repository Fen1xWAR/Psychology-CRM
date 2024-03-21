namespace CRM.Domain.Models;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public Guid DataId { get; set; }
}