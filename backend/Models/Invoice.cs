namespace backend.Models;

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public decimal TotalNet { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalGross { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    public string? MyDataUid { get; set; }
    public string? MyDataMark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
}
