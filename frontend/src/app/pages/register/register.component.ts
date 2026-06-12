import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TranslatePipe],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  form = {
    username: '',
    password: '',
    email: '',
    afm: '',
    businessName: ''
  };
  error = '';
  loading = false;

  constructor(private auth: AuthService, private router: Router) {}

  submit() {
    this.error = '';
    this.loading = true;
    this.auth.register(this.form).subscribe({
      next: () => this.router.navigate(['/']),
      error: () => {
        this.error = 'register.error';
        this.loading = false;
      }
    });
  }
}
