﻿namespace CRM.Domain.Models;

public class Contact
{
    public Guid ContactId { get; set; }
    public string PhoneNumber { get; set; }
    
    public string Name { get; set; }
    public string Lastname { get; set; }
    
    public string Middlename { get; set; }
    
    public DateOnly DateOfBirth { get; set; }
}