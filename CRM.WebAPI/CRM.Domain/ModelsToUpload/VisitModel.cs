using System.ComponentModel.DataAnnotations;

namespace CRM.Domain.ModelsToUpload;

public class VisitModel
{
    public Guid PsychologistId { get; set; } = Guid.Empty;
    public Guid ClientId { get; set; } = Guid.Empty;
    public Guid ServiceId { get; set; } = Guid.Empty;

    public string ClientNote { get; set; } = "";
    public string PsychologistDescription { get; set; } = "";
    public Guid ScheduleId { get; set; } = Guid.Empty;
}