using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class InvoiceService
{
    private static readonly Dictionary<int, decimal> VatRates = new()
    {
        { 1, 0.24m },
        { 2, 0.13m },
        { 3, 0.06m },
        { 7, 0m },
        { 8, 0m }
    };

    private readonly AppDbContext _db;
    private readonly MyDataService _myData;

    public InvoiceService(AppDbContext db, MyDataService myData)
    {
        _db = db;
        _myData = myData;
    }

    public async Task<List<Invoice>> GetInvoices(string username)
    {
        return await _db.Invoices
            .Include(i => i.Client)
            .Where(i => i.User.Username == username)
            .OrderByDescending(i => i.IssueDate)
            .ToListAsync();
    }

    public async Task<Invoice> GetInvoice(int id, string username)
    {
        return await _db.Invoices
            .Include(i => i.Client)
            .Include(i => i.Lines)
            .FirstOrDefaultAsync(i => i.Id == id && i.User.Username == username)
            ?? throw new Exception("Invoice not found");
    }

    public async Task<Invoice> CreateAndSubmit(InvoiceRequest request, string username)
    {
        var user = await _db.Users.FirstAsync(u => u.Username == username);
        var client = await _db.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId && c.UserId == user.Id)
            ?? throw new Exception("Client not found");

        var invoice = new Invoice
        {
            Series = request.Series,
            Aa = request.Aa,
            IssueDate = request.IssueDate,
            InvoiceType = request.InvoiceType,
            UserId = user.Id,
            User = user,
            ClientId = client.Id,
            Client = client
        };

        decimal totalNet = 0;
        decimal totalVat = 0;
        int lineNum = 1;

        foreach (var lr in request.Lines)
        {
            var net = Math.Round(lr.Quantity * lr.UnitPrice, 2);
            var vatRate = VatRates.GetValueOrDefault(lr.VatCategory, 0m);
            var vat = Math.Round(net * vatRate, 2);

            invoice.Lines.Add(new InvoiceLine
            {
                LineNumber = lineNum++,
                Description = lr.Description,
                Quantity = lr.Quantity,
                UnitPrice = lr.UnitPrice,
                VatCategory = lr.VatCategory,
                NetAmount = net,
                VatAmount = vat
            });

            totalNet += net;
            totalVat += vat;
        }

        invoice.TotalNet = totalNet;
        invoice.TotalVat = totalVat;
        invoice.TotalGross = totalNet + totalVat;
        invoice.Status = InvoiceStatus.Pending;

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();

        var result = await _myData.SendInvoice(invoice);
        if (result.Success)
        {
            invoice.Status = InvoiceStatus.Accepted;
            invoice.MyDataMark = result.Mark;
        }
        else
        {
            invoice.Status = InvoiceStatus.Rejected;
            invoice.MyDataError = result.Error;
        }

        await _db.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice> CancelInvoice(int id, string username)
    {
        var invoice = await GetInvoice(id, username);

        if (invoice.MyDataMark == null)
            throw new Exception("Invoice has no myDATA mark, cannot cancel");

        var result = await _myData.CancelInvoice(invoice.MyDataMark.Value);
        if (result.Success)
        {
            invoice.Status = InvoiceStatus.Cancelled;
            await _db.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"myDATA cancellation failed: {result.Error}");
        }

        return invoice;
    }

    public async Task<Invoice> ResubmitInvoice(int id, string username)
    {
        var invoice = await GetInvoice(id, username);

        if (invoice.Status != InvoiceStatus.Rejected)
            throw new Exception("Only rejected invoices can be resubmitted");

        invoice.Status = InvoiceStatus.Pending;
        invoice.MyDataError = null;
        await _db.SaveChangesAsync();

        var result = await _myData.SendInvoice(invoice);
        if (result.Success)
        {
            invoice.Status = InvoiceStatus.Accepted;
            invoice.MyDataMark = result.Mark;
        }
        else
        {
            invoice.Status = InvoiceStatus.Rejected;
            invoice.MyDataError = result.Error;
        }

        await _db.SaveChangesAsync();
        return invoice;
    }
}
