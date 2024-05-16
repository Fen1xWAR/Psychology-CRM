

namespace CRM.Domain.ModelsToUpload;

public class ContactModel
{
    public string PhoneNumber { get; set; } = "";
    public string Name { get; set; } = "";
    public string Lastname { get; set; } = "";
        
    public string Middlename { get; set; } = "";
    public DateOnly  DateOfBirth { get; set; } = new DateOnly();
}