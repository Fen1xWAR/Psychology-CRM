namespace CRM.Domain.ModelsToUpload;

public class UserAuth
{
    public string? Email { get; set; } = null;
    public string? Password { get; set; } = null;

    public Guid DeviceId { get; set; } = Guid.Empty;
}

