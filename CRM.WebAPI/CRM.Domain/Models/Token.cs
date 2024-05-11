namespace CRM.Domain.Models;

public class Token
{
    public Guid TokenId { get; set; }
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiredDateTime { get; set; }
    public Guid DeviceId { get; set; }
    
}