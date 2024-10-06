

namespace CRM.Domain.ModelsToUpload;

public class ClientModel
{
    public Guid FormId { get; set; } = Guid.Empty;
    public string CurrentProblem { get; set; } = "";
    public Guid UserId { get; set; } = Guid.Empty;
  
}