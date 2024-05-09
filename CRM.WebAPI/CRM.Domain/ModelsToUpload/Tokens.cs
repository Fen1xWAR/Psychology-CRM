using System.Text.Json.Serialization;

namespace CRM.Domain.ModelsToUpload;

public class Tokens
{
    public string JWTToken { get; set; }
    
    [JsonIgnore]
    public string RefreshToken { get; set; }

    
}