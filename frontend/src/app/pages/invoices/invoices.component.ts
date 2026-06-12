import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { InvoiceService, Invoice } from '../../services/invoice.service';

@Component({
  selector: 'app-invoices',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslatePipe],
  templateUrl: './invoices.component.html'
})
export class InvoicesComponent implements OnInit {
  invoices: Invoice[] = [];

  constructor(private invoiceService: InvoiceService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.invoiceService.getInvoices().subscribe(data => this.invoices = data);
  }

  cancel(id: number) {
    if (!confirm('Cancel this invoice?')) return;
    this.invoiceService.cancelInvoice(id).subscribe(() => this.load());
  }

  resubmit(id: number) {
    this.invoiceService.resubmitInvoice(id).subscribe(() => this.load());
  }

  statusClass(status: string | undefined): string {
    const map: Record<string, string> = {
      Accepted: 'bg-green-100 text-green-700 dark:bg-green-900/40 dark:text-green-400',
      Rejected: 'bg-red-100 text-red-700 dark:bg-red-900/40 dark:text-red-400',
      Cancelled: 'bg-zinc-100 text-zinc-500 dark:bg-zinc-700 dark:text-zinc-400',
      Pending: 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900/40 dark:text-yellow-400',
    };
    return map[status || 'Pending'] || map['Pending'];
  }
}
