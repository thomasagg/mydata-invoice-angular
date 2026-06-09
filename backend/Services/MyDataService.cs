using backend.Models;
using System.Xml;

namespace backend.Services;

public class MyDataService
{
    private static readonly string NS_INV  = "http://www.aade.gr/myDATA/invoice/v1.0";
    private static readonly string NS_ICLS = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0";

    private readonly string _userId;
    private readonly string _subscriptionKey;
    private readonly string _baseUrl;
    private readonly ILogger<MyDataService> _logger;
    private readonly HttpClient _http;

    public record MyDataResult(bool Success, long? Mark, string? Error);

    public MyDataService(IConfiguration config, ILogger<MyDataService> logger, HttpClient http)
    {
        _userId = config["MyData:UserId"]!;
        _subscriptionKey = config["MyData:SubscriptionKey"]!;
        _baseUrl = config["MyData:BaseUrl"]!;
        _logger = logger;
        _http = http;
    }

    public async Task<MyDataResult> SendInvoice(Invoice invoice)
    {
        try
        {
            var xml = BuildInvoiceXml(invoice);
            _logger.LogInformation("Sending XML to myDATA:\n{xml}", xml);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/SendInvoices");
            request.Headers.Add("aade-user-id", _userId);
            request.Headers.Add("ocp-apim-subscription-key", _subscriptionKey);
            request.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");

            var response = await _http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("myDATA response:\n{body}", body);

            return ParseResponse(body);
        }
        catch (Exception ex)
        {
            return new MyDataResult(false, null, ex.Message);
        }
    }

    public async Task<MyDataResult> CancelInvoice(long mark)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/CancelInvoice?mark={mark}");
            request.Headers.Add("aade-user-id", _userId);
            request.Headers.Add("ocp-apim-subscription-key", _subscriptionKey);

            var response = await _http.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            return ParseResponse(body);
        }
        catch (Exception ex)
        {
            return new MyDataResult(false, null, ex.Message);
        }
    }

    private string BuildInvoiceXml(Invoice invoice)
    {
        var iclsCategory = invoice.InvoiceType.StartsWith("2.") ? "category1_3" : "category1_1";

        var doc = new XmlDocument();
        var root = doc.CreateElement("InvoicesDoc", NS_INV);
        doc.AppendChild(root);

        var inv = doc.CreateElement("invoice", NS_INV);
        root.AppendChild(inv);

        var issuer = doc.CreateElement("issuer", NS_INV);
        AddEl(doc, issuer, "vatNumber", invoice.User.Afm);
        AddEl(doc, issuer, "country", "GR");
        AddEl(doc, issuer, "branch", "0");
        inv.AppendChild(issuer);

        var counterpart = doc.CreateElement("counterpart", NS_INV);
        AddEl(doc, counterpart, "vatNumber", invoice.Client.Afm);
        AddEl(doc, counterpart, "country", invoice.Client.Country ?? "GR");
        AddEl(doc, counterpart, "branch", "0");
        inv.AppendChild(counterpart);

        var header = doc.CreateElement("invoiceHeader", NS_INV);
        AddEl(doc, header, "series", invoice.Series);
        AddEl(doc, header, "aa", invoice.Aa);
        AddEl(doc, header, "issueDate", invoice.IssueDate.ToString("yyyy-MM-dd"));
        AddEl(doc, header, "invoiceType", invoice.InvoiceType);
        AddEl(doc, header, "currency", invoice.Currency);
        inv.AppendChild(header);

        var paymentMethods = doc.CreateElement("paymentMethods", NS_INV);
        var paymentMethod = doc.CreateElement("paymentMethodDetails", NS_INV);
        AddEl(doc, paymentMethod, "type", "3");
        AddEl(doc, paymentMethod, "amount", invoice.TotalGross.ToString("F2"));
        paymentMethods.AppendChild(paymentMethod);
        inv.AppendChild(paymentMethods);

        foreach (var line in invoice.Lines)
        {
            var details = doc.CreateElement("invoiceDetails", NS_INV);
            AddEl(doc, details, "lineNumber", line.LineNumber.ToString());
            AddEl(doc, details, "netValue", line.NetAmount.ToString("F2"));
            AddEl(doc, details, "vatCategory", line.VatCategory.ToString());
            AddEl(doc, details, "vatAmount", line.VatAmount.ToString("F2"));

            var lineIcls = doc.CreateElement("incomeClassification", NS_INV);
            AddIclsEl(doc, lineIcls, "classificationType", "E3_561_001");
            AddIclsEl(doc, lineIcls, "classificationCategory", iclsCategory);
            AddIclsEl(doc, lineIcls, "amount", line.NetAmount.ToString("F2"));
            details.AppendChild(lineIcls);

            inv.AppendChild(details);
        }

        var summary = doc.CreateElement("invoiceSummary", NS_INV);
        AddEl(doc, summary, "totalNetValue", invoice.TotalNet.ToString("F2"));
        AddEl(doc, summary, "totalVatAmount", invoice.TotalVat.ToString("F2"));
        AddEl(doc, summary, "totalWithheldAmount", "0.00");
        AddEl(doc, summary, "totalFeesAmount", "0.00");
        AddEl(doc, summary, "totalStampDutyAmount", "0.00");
        AddEl(doc, summary, "totalOtherTaxesAmount", "0.00");
        AddEl(doc, summary, "totalDeductionsAmount", "0.00");
        AddEl(doc, summary, "totalGrossValue", invoice.TotalGross.ToString("F2"));

        var icls = doc.CreateElement("incomeClassification", NS_INV);
        AddIclsEl(doc, icls, "classificationType", "E3_561_001");
        AddIclsEl(doc, icls, "classificationCategory", iclsCategory);
        AddIclsEl(doc, icls, "amount", invoice.TotalNet.ToString("F2"));
        summary.AppendChild(icls);

        inv.AppendChild(summary);

        using var sw = new StringWriter();
        doc.Save(sw);
        return sw.ToString();
    }

    private MyDataResult ParseResponse(string xml)
    {
        try
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var markNodes = doc.GetElementsByTagName("invoiceMark");
            if (markNodes.Count > 0)
            {
                var mark = long.Parse(markNodes[0]!.InnerText.Trim());
                return new MyDataResult(true, mark, null);
            }

            var errorNodes = doc.GetElementsByTagName("message");
            if (errorNodes.Count == 0) errorNodes = doc.GetElementsByTagName("errors");
            var error = errorNodes.Count > 0 ? errorNodes[0]!.InnerText.Trim() : "Unknown error from myDATA";
            return new MyDataResult(false, null, error);
        }
        catch (Exception ex)
        {
            return new MyDataResult(false, null, $"Failed to parse myDATA response: {ex.Message}");
        }
    }

    private void AddEl(XmlDocument doc, XmlElement parent, string tag, string value)
    {
        var el = doc.CreateElement(tag, NS_INV);
        el.InnerText = value;
        parent.AppendChild(el);
    }

    private void AddIclsEl(XmlDocument doc, XmlElement parent, string tag, string value)
    {
        var el = doc.CreateElement(tag, NS_ICLS);
        el.InnerText = value;
        parent.AppendChild(el);
    }
}
