namespace CRM.Domain.ModelsToUpload;

public class ScheduleModel
{
    public Guid PsychologistId { get; set; } = Guid.Empty;
    public DateOnly WorkDay { get; set; } = new DateOnly();
    public TimeOnly? StartTime { get; set; } = null;
    public TimeOnly? EndTime { get; set; } = null;
    public Guid? VisitId { get; set; } = null;
}