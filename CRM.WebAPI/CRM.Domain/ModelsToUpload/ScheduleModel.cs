namespace CRM.Domain.ModelsToUpload;

public class ScheduleModel
{
    public Guid PsychologistId { get; set; } = Guid.Empty;
    public DateOnly? WorkDay { get; set; } = null;
    public TimeOnly? StartTime { get; set; } = null;
    public TimeOnly? EndTime { get; set; } = null;
}