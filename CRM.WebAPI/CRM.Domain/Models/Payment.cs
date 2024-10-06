namespace CRM.Domain.Models;

public class Payment
{
    public Guid PaymentId { get; set; }
    public Guid ClientId { get; set; }
    public Guid VisitId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal PaymentAmount { get; set; }
    public string PaymentMethod { get; set; }
}