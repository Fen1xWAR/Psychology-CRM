namespace CRM.WebAPI.ModelsToUpload;

public class FileToUpload
{
    public Guid ClientId { get; set; }
    public Guid PsychologistId { get; set; }
    public IFormFile Files { get; set; }
}