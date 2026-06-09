using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/invoices")]
[Authorize]
public class InvoiceController : ControllerBase
{
    private readonly InvoiceService _invoiceService;

    public InvoiceController(InvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices()
    {
        var invoices = await _invoiceService.GetInvoices(User.Identity!.Name!);
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoice(int id)
    {
        try
        {
            var invoice = await _invoiceService.GetInvoice(id, User.Identity!.Name!);
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var invoice = await _invoiceService.CreateAndSubmit(request, User.Identity!.Name!);
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelInvoice(int id)
    {
        try
        {
            var invoice = await _invoiceService.CancelInvoice(id, User.Identity!.Name!);
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/resubmit")]
    public async Task<IActionResult> ResubmitInvoice(int id)
    {
        try
        {
            var invoice = await _invoiceService.ResubmitInvoice(id, User.Identity!.Name!);
            return Ok(invoice);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
