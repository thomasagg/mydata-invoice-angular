import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent],
  template: `
    <div class="flex h-screen bg-zinc-100 overflow-hidden">
      <app-sidebar />
      <main class="flex-1 overflow-y-auto">
        <router-outlet />
      </main>
    </div>
  `
})
export class MainLayoutComponent {}
