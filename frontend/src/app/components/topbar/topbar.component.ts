import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateService } from '@ngx-translate/core';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './topbar.component.html'
})
export class TopbarComponent {
  constructor(public theme: ThemeService, public translate: TranslateService) {}

  get lang() { return this.translate.currentLang() || 'el'; }

  toggleLang() {
    const next = this.lang === 'el' ? 'en' : 'el';
    this.translate.use(next);
    localStorage.setItem('lang', next);
  }
}
