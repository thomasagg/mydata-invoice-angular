namespace backend.Models;

public class InvoiceLine
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int VatCategory { get; set; } = 1;
    public decimal VatRate { get; set; }
    public decimal NetAmount { get; set; }
    public decimal VatAmount { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
}
