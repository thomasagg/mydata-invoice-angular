using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class InvoiceRequest
{
    [Required]
    public string Series { get; set; } = string.Empty;

    [Required]
    public string Aa { get; set; } = string.Empty;

    [Required]
    public DateOnly IssueDate { get; set; }

    public string InvoiceType { get; set; } = "1.1";

    [Required]
    public int ClientId { get; set; }

    [Required, MinLength(1)]
    public List<LineRequest> Lines { get; set; } = new();

    public class LineRequest
    {
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int VatCategory { get; set; }
    }
}
