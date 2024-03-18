namespace CRM.Domain.Models;

public class Visit
{
    public Guid VisitId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime DateTime { get; set; }
    public string ClientNote { get; set; }
    public string PsychologistDescription { get; set; }
    public Guid ServiceId { get; set; }
    public Guid PsychologistId { get; set; }
}