namespace backend.Models;

public class Invoice
{
    public int Id { get; set; }
    public string Series { get; set; } = string.Empty;
    public string Aa { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public string InvoiceType { get; set; } = "1.1";
    public string Currency { get; set; } = "EUR";
    public decimal TotalNet { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalGross { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
    public long? MyDataMark { get; set; }
    public string? MyDataError { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
}
