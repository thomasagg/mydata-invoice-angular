using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace backend.Models;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Afm { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string Country { get; set; } = "GR";

    public int UserId { get; set; }
    [ValidateNever, JsonIgnore]
    public User User { get; set; } = null!;
    [ValidateNever, JsonIgnore]
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
