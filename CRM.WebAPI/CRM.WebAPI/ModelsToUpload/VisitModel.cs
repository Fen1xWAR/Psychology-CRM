using Microsoft.Build.Framework;

namespace CRM.WebAPI.ModelsToUpload;

public class VisitModel
{
    [Required]
    public Guid PsychologistId { get; set; }
    [Required]
    public Guid ClientId { get; set; }
    [Required]
    public DateTime DateTime { get; set; }
    [Required]
    public Guid ServiceId {get; set; }

    public string? ClientNote  { get; set; }
    public string? Description  { get; set; }
    
}