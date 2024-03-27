namespace CRM.Domain.Models;

public class Schedule
{
    public Guid ScheduleId { get; set; }
    public Guid PsychologistId { get; set; }
    public DateOnly WorkDay { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}