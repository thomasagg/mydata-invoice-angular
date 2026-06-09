import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {
  navItems = [
    { to: '/', label: 'Dashboard', exact: true },
    { to: '/clients', label: 'Clients', exact: false },
    { to: '/invoices', label: 'Invoices', exact: false }
  ];

  constructor(private auth: AuthService, private router: Router) {}

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
