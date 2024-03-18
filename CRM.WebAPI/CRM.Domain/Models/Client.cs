namespace CRM.Domain.Models;

public class Client
{
    public Guid ClientId { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public Guid Form { get; set; }
    public string CurrentProblem { get; set; }
    public Guid ContactId { get; set; }
}