namespace CRM.Domain.ModelsToUpload;

public class LoginModel
{
    public Guid UserId { get; set; } = Guid.Empty;
    public DateTime? LoginTime { get; set; } = null;

}