namespace CRM.Domain.ModelsToUpload;

public class UserModel
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
    public Guid ContactId { get; set; } = Guid.Empty;
}