namespace CRM.Domain.ModelsToUpload;

public class ServiceModel
{
    public string? ServiceName { get; set; } = null;
    public decimal? ServicePrice { get; set; } = null;
    public Guid PsychologistId { get; set; } = Guid.Empty;
}