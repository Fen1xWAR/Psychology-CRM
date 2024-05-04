namespace CRM.Domain.Models;

public class Psychologist
{
    public Guid PsychologistId { get; set; }
    public Guid ContactId { get; set; }
    public Guid UserId { get; set; }
}