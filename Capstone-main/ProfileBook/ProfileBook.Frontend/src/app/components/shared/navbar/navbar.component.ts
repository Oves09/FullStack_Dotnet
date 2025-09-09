import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { NotificationService } from '../../../services/notification.service';
import { User } from '../../../models/user.model';
import { environment } from '../../../../environments/environment';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-light bg-white fixed-top shadow-sm">
      <div class="container">
        <a class="navbar-brand fw-bold" routerLink="/feed">
          <i class="fas fa-users me-2"></i>ProfileBook
        </a>

        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <a class="nav-link" routerLink="/feed" routerLinkActive="active">
                <i class="fas fa-home me-1"></i>Feed
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" routerLink="/messages" routerLinkActive="active">
                <i class="fas fa-envelope me-1"></i>Messages
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" routerLink="/search" routerLinkActive="active">
                <i class="fas fa-search me-1"></i>Search
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" routerLink="/my-groups" routerLinkActive="active">
                <i class="fas fa-users me-1"></i>My Groups
              </a>
            </li>
            <li class="nav-item" *ngIf="isAdmin">
              <a class="nav-link" routerLink="/admin" routerLinkActive="active">
                <i class="fas fa-cog me-1"></i>Admin
              </a>
            </li>
          </ul>

          <ul class="navbar-nav">
            <li class="nav-item" *ngIf="currentUser">
              <a class="nav-link position-relative" routerLink="/notifications">
                <i class="fas fa-bell"></i>
                <span *ngIf="unreadCount > 0" class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                  {{ unreadCount > 99 ? '99+' : unreadCount }}
                  <span class="visually-hidden">unread notifications</span>
                </span>
              </a>
            </li>
            <li class="nav-item dropdown" *ngIf="currentUser">
              <a class="nav-link dropdown-toggle d-flex align-items-center" href="#" id="navbarDropdown" 
                 role="button" data-bs-toggle="dropdown">
                <img [src]="getUserAvatar()" class="rounded-circle me-2" width="32" height="32" alt="Profile">
                {{ currentUser?.firstName }} {{ currentUser?.lastName }}
              </a>
              <ul class="dropdown-menu dropdown-menu-end">
                <li>
                  <a class="dropdown-item" routerLink="/profile">
                    <i class="fas fa-user me-2"></i>My Profile
                  </a>
                </li>
                <li>
                  <a class="dropdown-item" routerLink="/my-posts">
                    <i class="fas fa-file-alt me-2"></i>My Posts
                  </a>
                </li>
                <li>
                  <a class="dropdown-item" routerLink="/reports">
                    <i class="fas fa-flag me-2"></i>My Reports
                  </a>
                </li>
                <li><hr class="dropdown-divider"></li>
                <li>
                  <a class="dropdown-item" href="#" (click)="logout($event)">
                    <i class="fas fa-sign-out-alt me-2"></i>Logout
                  </a>
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  `,
  styles: [`
    .navbar {
      z-index: 1030;
    }

    .navbar-brand {
      font-size: 1.5rem;
      color: var(--primary-color) !important;
    }

    .nav-link {
      font-weight: 500;
      transition: color 0.2s ease;
    }

    .nav-link:hover {
      color: var(--primary-color) !important;
    }

    .nav-link.active {
      color: var(--primary-color) !important;
    }

    .dropdown-menu {
      border: none;
      box-shadow: var(--shadow-lg);
      border-radius: 0.5rem;
    }

    .dropdown-item {
      padding: 0.75rem 1rem;
      transition: background-color 0.2s ease;
    }

    .dropdown-item:hover {
      background-color: var(--light-bg);
    }

    @media (max-width: 768px) {
      .navbar-nav {
        text-align: center;
      }
      
      .dropdown-menu {
        position: static !important;
        transform: none !important;
        border: none;
        box-shadow: none;
        background-color: var(--light-bg);
        margin-top: 0.5rem;
      }
    }
  `]
})
export class NavbarComponent implements OnInit, OnDestroy {
  currentUser: User | null = null;
  unreadCount = 0;
  private subscriptions: Subscription[] = [];
  isAdmin = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    const userSub = this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.isAdmin = this.authService.isAdmin();
      
      if (user) {
        // Subscribe to notification count updates
        const notificationSub = this.notificationService.unreadCount$.subscribe(count => {
          this.unreadCount = count;
        });
        this.subscriptions.push(notificationSub);
        
        // Initial load of notification count
        this.notificationService.refreshUnreadCount();
      }
    });
    this.subscriptions.push(userSub);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  getUserAvatar(): string {
    if (this.currentUser?.profileImagePath) {
      return this.getFullImageUrl(this.currentUser.profileImagePath);
    }
    return 'https://via.placeholder.com/32x32/3b82f6/ffffff?text=' + 
           (this.currentUser?.firstName?.charAt(0) || this.currentUser?.userName?.charAt(0) || 'U');
  }

  getFullImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    if (imagePath.startsWith('http')) return imagePath;
    return `http://localhost:5120${imagePath}`;
  }

  logout(event: Event) {
    event.preventDefault();
    this.authService.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.error('Logout error:', error);
        // Force logout even if API call fails
        this.router.navigate(['/login']);
      }
    });
  }
}
