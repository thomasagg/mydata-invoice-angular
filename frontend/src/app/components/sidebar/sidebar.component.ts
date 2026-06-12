import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule, TranslatePipe],
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {
  navItems = [
    { to: '/', label: 'nav.dashboard', exact: true },
    { to: '/clients', label: 'nav.clients', exact: false },
    { to: '/invoices', label: 'nav.invoices', exact: false }
  ];

  constructor(private auth: AuthService, private router: Router) {}

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
