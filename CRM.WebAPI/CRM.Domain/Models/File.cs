namespace CRM.Domain.Models;

public class File
{
    public Guid FileId { get; set; }
    public Guid ClientId { get; set; }
    public Guid PsychologistId { get; set; }
    public string FileName { get; set; }
    public byte[] FileContent { get; set; }
}