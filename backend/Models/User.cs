namespace backend.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Afm { get; set; } = string.Empty;
    public string? BusinessName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Client> Clients { get; set; } = new List<Client>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
