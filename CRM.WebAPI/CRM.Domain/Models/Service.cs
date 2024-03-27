namespace CRM.Domain.Models;

public class Service
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; }
    public decimal ServicePrice { get; set; }
    public string ServiceDescription { get; set; }
}