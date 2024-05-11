using System.Text.Json.Serialization;

namespace CRM.Domain.ModelsToUpload;

public class Tokens
{
    public string JWTToken { get; set; }
    
    public RefreshToken RefreshToken { get; set; }

    
}