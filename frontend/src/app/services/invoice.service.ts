import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface InvoiceLine {
  description: string;
  quantity: number;
  unitPrice: number;
  vatCategory: number;
}

export interface Invoice {
  id?: number;
  series: string;
  aa: string;
  issueDate: string;
  invoiceType: string;
  clientId: number;
  totalNet?: number;
  totalVat?: number;
  totalGross?: number;
  status?: string;
  myDataMark?: number;
  myDataError?: string;
  client?: { name: string; afm: string };
  lines?: InvoiceLine[];
}

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private baseUrl = '/api/invoices';

  constructor(private http: HttpClient) {}

  getInvoices() {
    return this.http.get<Invoice[]>(this.baseUrl);
  }

  createInvoice(invoice: Invoice & { lines: InvoiceLine[] }) {
    return this.http.post<Invoice>(this.baseUrl, invoice);
  }

  cancelInvoice(id: number) {
    return this.http.post<Invoice>(`${this.baseUrl}/${id}/cancel`, {});
  }

  resubmitInvoice(id: number) {
    return this.http.post<Invoice>(`${this.baseUrl}/${id}/resubmit`, {});
  }
}
