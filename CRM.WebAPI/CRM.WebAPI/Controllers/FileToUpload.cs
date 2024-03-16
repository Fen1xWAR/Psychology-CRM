namespace CRM.WebAPI.Controllers;

public class FileToUpload
{
    // public Guid ClientId { get; set; }
    public IFormFile files { get; set; }
}