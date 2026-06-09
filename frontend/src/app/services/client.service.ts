import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface Client {
  id?: number;
  name: string;
  afm: string;
  address?: string;
  city?: string;
  country: string;
}

@Injectable({ providedIn: 'root' })
export class ClientService {
  private baseUrl = '/api/clients';

  constructor(private http: HttpClient) {}

  getClients() {
    return this.http.get<Client[]>(this.baseUrl);
  }

  createClient(client: Client) {
    return this.http.post<Client>(this.baseUrl, client);
  }

  updateClient(id: number, client: Client) {
    return this.http.put<Client>(`${this.baseUrl}/${id}`, client);
  }

  deleteClient(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
