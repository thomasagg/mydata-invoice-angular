import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceService, Invoice } from '../../services/invoice.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  invoices: Invoice[] = [];

  constructor(private invoiceService: InvoiceService) {}

  ngOnInit() {
    this.invoiceService.getInvoices().subscribe(data => this.invoices = data);
  }

  get total() { return this.invoices.length; }
  get accepted() { return this.invoices.filter(i => i.status === 'Accepted').length; }
  get rejected() { return this.invoices.filter(i => i.status === 'Rejected').length; }
  get cancelled() { return this.invoices.filter(i => i.status === 'Cancelled').length; }
}
