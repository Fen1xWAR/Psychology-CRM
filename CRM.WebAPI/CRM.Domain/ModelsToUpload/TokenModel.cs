namespace CRM.Domain.ModelsToUpload;

public class TokenModel
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string RefreshToken { get; set; } = "";

    public DateTime ExpiredDateTime { get; set; } = DateTime.Now.AddDays(7);
    public Guid DeviceId { get; set; } = Guid.Empty;
}