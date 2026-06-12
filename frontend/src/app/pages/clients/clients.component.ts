import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslatePipe } from '@ngx-translate/core';
import { ClientService, Client } from '../../services/client.service';

const empty: Client = { name: '', afm: '', address: '', city: '', country: 'GR' };

@Component({
  selector: 'app-clients',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './clients.component.html'
})
export class ClientsComponent implements OnInit {
  clients: Client[] = [];
  form: Client = { ...empty };
  editingId: number | null = null;
  showForm = false;
  error = '';

  constructor(private clientService: ClientService) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.clientService.getClients().subscribe(clients => this.clients = clients);
  }

  openAdd() {
    this.form = { ...empty };
    this.editingId = null;
    this.showForm = true;
  }

  openEdit(c: Client) {
    this.form = { name: c.name, afm: c.afm, address: c.address || '', city: c.city || '', country: c.country || 'GR' };
    this.editingId = c.id!;
    this.showForm = true;
  }

  submit() {
    this.error = '';
    const call = this.editingId
      ? this.clientService.updateClient(this.editingId, this.form)
      : this.clientService.createClient(this.form);

    call.subscribe({
      next: () => {
        this.showForm = false;
        this.editingId = null;
        this.form = { ...empty };
        this.load();
      },
      error: () => this.error = 'clients.error'
    });
  }

  remove(id: number) {
    if (!confirm('Delete this client?')) return;
    this.clientService.deleteClient(id).subscribe(() => this.load());
  }
}
