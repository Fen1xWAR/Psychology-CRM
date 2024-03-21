namespace CRM.Domain.Models;

public class Psychologist
{
    public Guid PsychologistId { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public Guid ContactId { get; set; }
}