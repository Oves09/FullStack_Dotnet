import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { FooterComponent } from './components/shared/footer/footer.component';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavbarComponent, FooterComponent],
  template: `
    <div class="app-container">
      <app-navbar *ngIf="showNavbar"></app-navbar>
      <main class="main-content" [class.with-navbar]="showNavbar">
        <router-outlet></router-outlet>
      </main>
      <app-footer *ngIf="showNavbar"></app-footer>
    </div>
  `,
  styles: [`
    .app-container {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
    }

    .main-content {
      flex: 1;
      padding-top: 0;
    }

    .main-content.with-navbar {
      padding-top: 80px;
    }

    @media (max-width: 768px) {
      .main-content.with-navbar {
        padding-top: 70px;
      }
    }
  `]
})
export class AppComponent implements OnInit {
  showNavbar = true;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event) => {
        if (event instanceof NavigationEnd) {
          // Hide navbar on login and register pages
          this.showNavbar = !['/login', '/register'].includes(event.url);
        }
      });
  }
}
