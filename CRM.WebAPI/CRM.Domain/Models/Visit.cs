namespace CRM.Domain.Models;

public class Visit
{
    public Guid VisitId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime DateTime { get; set; }
    public string ClientNote { get; set; }
    public Guid ServiceID { get; set; }
    public Guid PsychologistID { get; set; }
    public Guid VisitNote { get; set; }
    
}