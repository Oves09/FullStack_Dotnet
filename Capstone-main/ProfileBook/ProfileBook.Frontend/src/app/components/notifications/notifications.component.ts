import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../../services/notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
              <h4 class="mb-0">
                <i class="fas fa-bell me-2"></i>Notifications
              </h4>
              <button 
                class="btn btn-light btn-sm" 
                (click)="markAllAsRead()"
                [disabled]="notifications.length === 0 || isLoading"
                *ngIf="hasUnreadNotifications()">
                <i class="fas fa-check-double me-1"></i>Mark All Read
              </button>
            </div>
            <div class="card-body p-0">
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading notifications...</span>
                </div>
                <p class="mt-2 text-muted">Loading notifications...</p>
              </div>

              <!-- Notifications List -->
              <div class="list-group list-group-flush" *ngIf="!isLoading && notifications.length > 0">
                <div 
                  *ngFor="let notification of notifications; let i = index"
                  class="list-group-item list-group-item-action"
                  [class.bg-light]="!notification.isRead"
                  (click)="markAsRead(notification, i)">
                  <div class="d-flex w-100 justify-content-between align-items-start">
                    <div class="flex-grow-1">
                      <div class="d-flex align-items-center mb-1">
                        <i [class]="getNotificationIcon(notification.type)" 
                           [ngClass]="getNotificationIconClass(notification.type)"></i>
                        <h6 class="mb-0 ms-2">{{ notification.title }}</h6>
                        <span 
                          *ngIf="!notification.isRead" 
                          class="badge bg-primary ms-2">New</span>
                      </div>
                      <p class="mb-1 text-muted">{{ notification.message }}</p>
                      <small class="text-muted">
                        <i class="fas fa-clock me-1"></i>{{ formatDate(notification.createdAt) }}
                      </small>
                    </div>
                    <button 
                      *ngIf="!notification.isRead"
                      class="btn btn-sm btn-outline-primary ms-2"
                      (click)="markAsRead(notification, i); $event.stopPropagation()">
                      <i class="fas fa-check"></i>
                    </button>
                  </div>
                </div>
              </div>

              <!-- Empty State -->
              <div class="text-center py-5" *ngIf="!isLoading && notifications.length === 0">
                <i class="fas fa-bell-slash fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No notifications</h5>
                <p class="text-muted">You're all caught up! No new notifications to show.</p>
              </div>

              <!-- Load More -->
              <div class="text-center p-3" *ngIf="!isLoading && notifications.length > 0 && hasMoreNotifications">
                <button 
                  class="btn btn-outline-primary"
                  (click)="loadMoreNotifications()"
                  [disabled]="isLoadingMore">
                  <span *ngIf="isLoadingMore" class="spinner-border spinner-border-sm me-2"></span>
                  <i *ngIf="!isLoadingMore" class="fas fa-chevron-down me-2"></i>
                  {{ isLoadingMore ? 'Loading...' : 'Load More' }}
                </button>
              </div>
            </div>
          </div>

          <!-- Success Message -->
          <div class="alert alert-success alert-dismissible fade show mt-3" *ngIf="successMessage">
            <i class="fas fa-check-circle me-2"></i>{{ successMessage }}
            <button type="button" class="btn-close" (click)="successMessage = ''"></button>
          </div>

          <!-- Error Message -->
          <div class="alert alert-danger alert-dismissible fade show mt-3" *ngIf="errorMessage">
            <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
            <button type="button" class="btn-close" (click)="errorMessage = ''"></button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .list-group-item {
      border-left: none;
      border-right: none;
      cursor: pointer;
      transition: background-color 0.2s ease;
    }

    .list-group-item:hover {
      background-color: var(--bs-gray-100) !important;
    }

    .list-group-item.bg-light {
      background-color: var(--bs-blue-50) !important;
      border-left: 3px solid var(--bs-primary);
    }

    .notification-icon {
      width: 20px;
      text-align: center;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .card-header h4 {
        font-size: 1.1rem;
      }
      
      .btn-sm {
        padding: 0.25rem 0.5rem;
        font-size: 0.75rem;
      }
    }
  `]
})
export class NotificationsComponent implements OnInit, OnDestroy {
  notifications: Notification[] = [];
  isLoading = false;
  isLoadingMore = false;
  successMessage = '';
  errorMessage = '';
  currentPage = 1;
  pageSize = 20;
  hasMoreNotifications = true;

  private subscriptions: Subscription[] = [];

  constructor(private notificationService: NotificationService) {}

  ngOnInit() {
    this.loadNotifications();
    this.notificationService.refreshUnreadCount();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  loadNotifications() {
    this.isLoading = true;
    this.errorMessage = '';

    const sub = this.notificationService.getNotifications(this.currentPage, this.pageSize).subscribe({
      next: (notifications) => {
        this.notifications = notifications;
        this.hasMoreNotifications = notifications.length === this.pageSize;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load notifications. Please try again.';
        console.error('Error loading notifications:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  loadMoreNotifications() {
    this.isLoadingMore = true;
    this.currentPage++;

    const sub = this.notificationService.getNotifications(this.currentPage, this.pageSize).subscribe({
      next: (notifications) => {
        this.notifications = [...this.notifications, ...notifications];
        this.hasMoreNotifications = notifications.length === this.pageSize;
        this.isLoadingMore = false;
      },
      error: (error) => {
        this.isLoadingMore = false;
        this.currentPage--; // Revert page increment on error
        this.errorMessage = 'Failed to load more notifications. Please try again.';
        console.error('Error loading more notifications:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  markAsRead(notification: Notification, index: number) {
    if (notification.isRead) return;

    const sub = this.notificationService.markAsRead(notification.notificationId).subscribe({
      next: () => {
        this.notifications[index].isRead = true;
        this.notificationService.refreshUnreadCount();
        this.successMessage = 'Notification marked as read';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        this.errorMessage = 'Failed to mark notification as read';
        console.error('Error marking notification as read:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  markAllAsRead() {
    const sub = this.notificationService.markAllAsRead().subscribe({
      next: () => {
        this.notifications.forEach(notification => notification.isRead = true);
        this.notificationService.refreshUnreadCount();
        this.successMessage = 'All notifications marked as read';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error) => {
        this.errorMessage = 'Failed to mark all notifications as read';
        console.error('Error marking all notifications as read:', error);
      }
    });

    this.subscriptions.push(sub);
  }

  hasUnreadNotifications(): boolean {
    return this.notifications.some(n => !n.isRead);
  }

  getNotificationIcon(type: string): string {
    switch (type.toLowerCase()) {
      case 'success':
        return 'fas fa-check-circle notification-icon';
      case 'warning':
        return 'fas fa-exclamation-triangle notification-icon';
      case 'error':
        return 'fas fa-times-circle notification-icon';
      case 'info':
        return 'fas fa-info-circle notification-icon';
      default:
        return 'fas fa-bell notification-icon';
    }
  }

  getNotificationIconClass(type: string): string {
    switch (type.toLowerCase()) {
      case 'success':
        return 'text-success';
      case 'warning':
        return 'text-warning';
      case 'error':
        return 'text-danger';
      case 'info':
        return 'text-info';
      default:
        return 'text-primary';
    }
  }

  formatDate(date: Date): string {
    const now = new Date();
    const notificationDate = new Date(date);
    const diffInMinutes = Math.floor((now.getTime() - notificationDate.getTime()) / (1000 * 60));

    if (diffInMinutes < 1) return 'Just now';
    if (diffInMinutes < 60) return `${diffInMinutes}m ago`;
    
    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) return `${diffInHours}h ago`;
    
    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays < 7) return `${diffInDays}d ago`;
    
    return notificationDate.toLocaleDateString();
  }
}
