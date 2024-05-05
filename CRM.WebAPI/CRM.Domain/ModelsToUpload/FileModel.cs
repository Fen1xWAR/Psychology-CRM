using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CRM.Domain.ModelsToUpload;

public class FileModel
{
     public Guid ClientId { get; set; } = Guid.Empty;
     public Guid PsychologistId { get; set; } = Guid.Empty;
     public IFormFile? Files { get; set; } = null;
}