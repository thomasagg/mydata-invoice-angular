import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { InvoiceService } from '../../services/invoice.service';
import { ClientService, Client } from '../../services/client.service';

const VAT_CATEGORIES = [
  { value: 1, label: '24%' },
  { value: 2, label: '13%' },
  { value: 3, label: '6%' },
  { value: 7, label: '0%' },
  { value: 8, label: 'Exempt' }
];

const INVOICE_TYPES = [
  { value: '1.1', label: '1.1 - Sales Invoice' },
  { value: '2.1', label: '2.1 - Service Invoice' }
];

const emptyLine = () => ({ description: '', quantity: 1, unitPrice: 0, vatCategory: 1 });

@Component({
  selector: 'app-new-invoice',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TranslatePipe],
  templateUrl: './new-invoice.component.html'
})
export class NewInvoiceComponent implements OnInit {
  vatCategories = VAT_CATEGORIES;
  invoiceTypes = INVOICE_TYPES;

  clients: Client[] = [];
  form = {
    series: 'A',
    aa: '',
    issueDate: new Date().toISOString().split('T')[0],
    invoiceType: '1.1',
    clientId: 0
  };
  lines = [emptyLine()];
  error = '';
  loading = false;

  constructor(
    private invoiceService: InvoiceService,
    private clientService: ClientService,
    private router: Router
  ) {}

  ngOnInit() {
    this.clientService.getClients().subscribe(clients => {
      this.clients = clients;
      if (clients.length > 0) this.form.clientId = clients[0].id!;
    });
  }

  calcLine(line: typeof this.lines[0]) {
    const rates: Record<number, number> = { 1: 0.24, 2: 0.13, 3: 0.06, 7: 0, 8: 0 };
    const net = (line.quantity || 0) * (line.unitPrice || 0);
    const vat = net * (rates[line.vatCategory] || 0);
    return { net: isNaN(net) ? 0 : net, vat: isNaN(vat) ? 0 : vat };
  }

  get totals() {
    return this.lines.reduce((acc, l) => {
      const { net, vat } = this.calcLine(l);
      return { net: acc.net + net, vat: acc.vat + vat };
    }, { net: 0, vat: 0 });
  }

  addLine() {
    this.lines.push(emptyLine());
  }

  removeLine(i: number) {
    this.lines.splice(i, 1);
  }

  submit() {
    this.error = '';
    this.loading = true;
    this.invoiceService.createInvoice({ ...this.form, lines: this.lines }).subscribe({
      next: () => this.router.navigate(['/invoices']),
      error: (err) => {
        this.error = err.error?.message || 'Failed to submit invoice';
        this.loading = false;
      }
    });
  }
}
