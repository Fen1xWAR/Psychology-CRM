namespace CRM.Domain.ModelsToUpload;

public class RefreshToken
{
    public string Token { get; set; } = "";
    public Guid DeviceId { get; set; } = Guid.Empty;
}