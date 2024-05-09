namespace CRM.Domain.ModelsToUpload;

public class TokenModel
{
    public Guid userId { get; set; } = Guid.Empty;
    public string RefreshToken { get; set; } = "";
}