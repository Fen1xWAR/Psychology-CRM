namespace CRM.Domain.Models;

public class Client
{
    public Guid ClientId { get; set; }
    public Guid FormId { get; set; }
    public string? CurrentProblem { get; set; }
    public Guid ContactId { get; set; }
    public Guid UserId { get; set; }
}