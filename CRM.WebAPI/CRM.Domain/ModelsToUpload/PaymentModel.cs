namespace CRM.Domain.ModelsToUpload;

public class PaymentModel
{
    public Guid ClientId { get; set; } = Guid.Empty;
    public Guid VisitId { get; set; } = Guid.Empty;
    public DateTime? PaymentDate { get; set; } = null;
    public decimal? PaymentAmount { get; set; } = null;
    public string? PaymentMethod { get; set; } = null;
}