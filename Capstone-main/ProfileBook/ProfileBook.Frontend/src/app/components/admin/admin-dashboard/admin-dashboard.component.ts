import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AdminService } from '../../../services/admin.service';
import { PostService } from '../../../services/post.service';
import { UserService } from '../../../services/user.service';
import { Post } from '../../../models/post.model';
import { User } from '../../../models/user.model';
import { Report, ReportStatus } from '../../../models/report.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container mt-4">
      <div class="row">
        <div class="col-12">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="text-primary">
              <i class="fas fa-tachometer-alt me-2"></i>Admin Dashboard
            </h2>
          </div>

          <!-- Statistics Cards -->
          <div class="row">
            <div class="col-md-3 mb-3">
              <div class="card text-center">
                <div class="card-body">
                  <i class="fas fa-file-alt fa-3x text-primary mb-3"></i>
                  <h5>Post Management</h5>
                  <p class="text-muted">Review and approve posts</p>
                  <a routerLink="/admin/posts" class="btn btn-primary">
                    <i class="fas fa-eye me-2"></i>View Posts
                  </a>
                </div>
              </div>
            </div>

            <div class="col-md-3 mb-3">
              <div class="card text-center">
                <div class="card-body">
                  <i class="fas fa-users fa-3x text-success mb-3"></i>
                  <h5>User Management</h5>
                  <p class="text-muted">Manage user accounts</p>
                  <a routerLink="/admin/users" class="btn btn-success">
                    <i class="fas fa-cog me-2"></i>Manage Users
                  </a>
                </div>
              </div>
            </div>

            <div class="col-md-3 mb-3">
              <div class="card text-center">
                <div class="card-body">
                  <i class="fas fa-flag fa-3x text-warning mb-3"></i>
                  <h5>Report Management</h5>
                  <p class="text-muted">Handle user reports</p>
                  <a routerLink="/admin/reports" class="btn btn-warning">
                    <i class="fas fa-search me-2"></i>View Reports
                  </a>
                </div>
              </div>
            </div>

            <div class="col-md-3 mb-3">
              <div class="card text-center">
                <div class="card-body">
                  <i class="fas fa-users-cog fa-3x text-info mb-3"></i>
                  <h5>Group Management</h5>
                  <p class="text-muted">Create and manage user groups</p>
                  <a routerLink="/admin/groups" class="btn btn-info">
                    <i class="fas fa-layer-group me-2"></i>Manage Groups
                  </a>
                </div>
              </div>
            </div>
          </div>

          <!-- Quick Actions -->
          <div class="row mb-4">
            <div class="col-12">
              <div class="card">
                <div class="card-header">
                  <h5 class="mb-0">
                    <i class="fas fa-bolt me-2"></i>Quick Actions
                  </h5>
                </div>
                <div class="card-body">
                  <div class="row">
                    <div class="col-md-3 mb-2">
                      <a routerLink="/admin/posts" class="btn btn-outline-primary w-100">
                        <i class="fas fa-file-alt me-2"></i>Review Posts
                      </a>
                    </div>
                    <div class="col-md-3 mb-2">
                      <a routerLink="/admin/users" class="btn btn-outline-success w-100">
                        <i class="fas fa-users me-2"></i>Manage Users
                      </a>
                    </div>
                    <div class="col-md-3 mb-2">
                      <a routerLink="/admin/reports" class="btn btn-outline-warning w-100">
                        <i class="fas fa-flag me-2"></i>Handle Reports
                      </a>
                    </div>
                    <div class="col-md-3 mb-2">
                      <button class="btn btn-outline-info w-100" (click)="refreshData()">
                        <i class="fas fa-sync-alt me-2"></i>Refresh Data
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Recent Activity -->
          <div class="row">
            <div class="col-md-6">
              <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <h5 class="mb-0">
                    <i class="fas fa-clock me-2"></i>Recent Pending Posts
                  </h5>
                  <a routerLink="/admin/posts" class="btn btn-sm btn-outline-primary">View All</a>
                </div>
                <div class="card-body">
                  <div *ngIf="isLoadingPosts" class="text-center py-3">
                    <div class="spinner-border spinner-border-sm text-primary"></div>
                    <span class="ms-2">Loading...</span>
                  </div>

                  <div *ngIf="!isLoadingPosts && recentPosts.length === 0" class="text-center py-3 text-muted">
                    <i class="fas fa-check-circle fa-2x mb-2"></i>
                    <p class="mb-0">No pending posts</p>
                  </div>

                  <div *ngFor="let post of recentPosts" class="border-bottom py-2">
                    <div class="d-flex align-items-start">
                      <img [src]="getPostUserAvatar(post)" class="rounded-circle me-2" 
                           width="32" height="32" alt="User avatar">
                      <div class="flex-grow-1 min-w-0">
                        <h6 class="mb-1 text-truncate">{{ post.userFullName }}</h6>
                        <p class="mb-1 text-muted small text-truncate">{{ post.content }}</p>
                        <small class="text-muted">{{ formatDate(post.createdAt) }}</small>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div class="col-md-6">
              <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <h5 class="mb-0">
                    <i class="fas fa-exclamation-triangle me-2"></i>Recent Reports
                  </h5>
                  <a routerLink="/admin/reports" class="btn btn-sm btn-outline-warning">View All</a>
                </div>
                <div class="card-body">
                  <div *ngIf="isLoadingReports" class="text-center py-3">
                    <div class="spinner-border spinner-border-sm text-warning"></div>
                    <span class="ms-2">Loading...</span>
                  </div>

                  <div *ngIf="!isLoadingReports && recentReports.length === 0" class="text-center py-3 text-muted">
                    <i class="fas fa-shield-alt fa-2x mb-2"></i>
                    <p class="mb-0">No pending reports</p>
                  </div>

                  <div *ngFor="let report of recentReports" class="border-bottom py-2">
                    <div class="d-flex justify-content-between align-items-start">
                      <div class="flex-grow-1 min-w-0">
                        <h6 class="mb-1">{{ report.reportedUserFullName }}</h6>
                        <p class="mb-1 text-muted small">{{ report.reason }}</p>
                        <small class="text-muted">by {{ report.reportingUserFullName }}</small>
                      </div>
                      <span class="badge bg-warning">{{ getStatusText(report.status) }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Error Messages -->
          <div class="alert alert-danger mt-3" *ngIf="errorMessage">
            <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
            <button type="button" class="btn-close" (click)="errorMessage = ''"></button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .stat-card {
      border: none;
      border-radius: 0.75rem;
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .stat-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    }

    .opacity-75 {
      opacity: 0.75;
    }

    .min-w-0 {
      min-width: 0;
    }

    .text-truncate {
      overflow: hidden;
      text-overflow: ellipsis;
      white-space: nowrap;
    }

    .border-bottom:last-child {
      border-bottom: none !important;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .stat-card h3 {
        font-size: 1.5rem;
      }
      
      .stat-card .fa-2x {
        font-size: 1.5em;
      }
      
      .btn.w-100 {
        margin-bottom: 0.5rem;
      }
    }
  `]
})
export class AdminDashboardComponent implements OnInit {
  stats = {
    pendingPosts: 0,
    totalUsers: 0,
    pendingReports: 0,
    approvedPosts: 0
  };

  recentPosts: Post[] = [];
  recentReports: Report[] = [];
  isLoadingPosts = false;
  isLoadingReports = false;
  errorMessage = '';

  constructor(
    private adminService: AdminService,
    private postService: PostService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.loadPendingPosts();
    this.loadRecentReports();
    this.loadUserStats();
  }

  loadPendingPosts() {
    this.isLoadingPosts = true;
    this.adminService.getPendingPosts().subscribe({
      next: (posts) => {
        this.recentPosts = posts.slice(0, 5); // Show only 5 recent posts
        this.stats.pendingPosts = posts.length;
        this.isLoadingPosts = false;
      },
      error: (error) => {
        this.isLoadingPosts = false;
        console.error('Error loading pending posts:', error);
      }
    });
  }

  loadRecentReports() {
    this.isLoadingReports = true;
    this.adminService.getReports(ReportStatus.Pending).subscribe({
      next: (reports) => {
        this.recentReports = reports.slice(0, 5); // Show only 5 recent reports
        this.stats.pendingReports = reports.length;
        this.isLoadingReports = false;
      },
      error: (error) => {
        this.isLoadingReports = false;
        console.error('Error loading reports:', error);
      }
    });
  }

  loadUserStats() {
    // Load basic user statistics
    this.adminService.getUsers(1, 1).subscribe({
      next: (users) => {
        // This is a simplified approach - in a real app you'd have a dedicated stats endpoint
        this.stats.totalUsers = 50; // Placeholder - would come from API
        this.stats.approvedPosts = 120; // Placeholder - would come from API
      },
      error: (error) => {
        console.error('Error loading user stats:', error);
      }
    });
  }

  refreshData() {
    this.errorMessage = '';
    this.loadDashboardData();
  }

  getPostUserAvatar(post: Post): string {
    if (post.userProfileImage) {
      return post.userProfileImage;
    }
    return 'https://via.placeholder.com/32x32/3b82f6/ffffff?text=' + 
           (post.userFullName?.charAt(0) || 'U');
  }

  getStatusText(status: ReportStatus): string {
    switch (status) {
      case ReportStatus.Pending:
        return 'Pending';
      case ReportStatus.UnderReview:
        return 'Under Review';
      case ReportStatus.Resolved:
        return 'Resolved';
      case ReportStatus.Dismissed:
        return 'Dismissed';
      default:
        return 'Unknown';
    }
  }

  formatDate(date: Date): string {
    const postDate = new Date(date);
    const now = new Date();
    const diffInMinutes = Math.floor((now.getTime() - postDate.getTime()) / (1000 * 60));

    if (diffInMinutes < 60) return `${diffInMinutes}m ago`;
    if (diffInMinutes < 1440) return `${Math.floor(diffInMinutes / 60)}h ago`;
    if (diffInMinutes < 10080) return `${Math.floor(diffInMinutes / 1440)}d ago`;
    
    return postDate.toLocaleDateString();
  }
}
