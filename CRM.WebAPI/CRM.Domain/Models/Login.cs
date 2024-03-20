namespace CRM.Domain.Models;

public class Login
{
    public Guid LoginId { get; set; }
    public Guid UserId { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime LogoutTime { get; set; }
}