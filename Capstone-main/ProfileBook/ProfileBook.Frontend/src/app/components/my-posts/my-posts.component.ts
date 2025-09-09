import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostService } from '../../services/post.service';
import { Post, PostStatus } from '../../models/post.model';

@Component({
  selector: 'app-my-posts',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">
                <i class="fas fa-file-alt me-2"></i>My Posts
              </h4>
            </div>
            <div class="card-body">
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading your posts...</span>
                </div>
                <p class="mt-2 text-muted">Loading your posts...</p>
              </div>

              <!-- No Posts -->
              <div class="text-center py-5" *ngIf="!isLoading && posts.length === 0">
                <i class="fas fa-file-alt fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No posts yet</h5>
                <p class="text-muted">You haven't created any posts yet. Start sharing your thoughts!</p>
                <a routerLink="/feed" class="btn btn-primary">
                  <i class="fas fa-plus me-2"></i>Create Your First Post
                </a>
              </div>

              <!-- Posts Statistics -->
              <div class="row mb-4" *ngIf="!isLoading && posts.length > 0">
                <div class="col-md-4">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-primary mb-1">{{ getTotalPosts() }}</h3>
                    <small class="text-muted">Total Posts</small>
                  </div>
                </div>
                <div class="col-md-4">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-success mb-1">{{ getApprovedPosts() }}</h3>
                    <small class="text-muted">Approved</small>
                  </div>
                </div>
                <div class="col-md-4">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-warning mb-1">{{ getPendingPosts() }}</h3>
                    <small class="text-muted">Pending</small>
                  </div>
                </div>
              </div>

              <!-- Filter Buttons -->
              <div class="mb-4" *ngIf="!isLoading && posts.length > 0">
                <div class="btn-group" role="group">
                  <button type="button" class="btn" 
                          [class.btn-primary]="selectedFilter === 'all'"
                          [class.btn-outline-primary]="selectedFilter !== 'all'"
                          (click)="setFilter('all')">
                    All Posts
                  </button>
                  <button type="button" class="btn"
                          [class.btn-success]="selectedFilter === 'approved'"
                          [class.btn-outline-success]="selectedFilter !== 'approved'"
                          (click)="setFilter('approved')">
                    Approved
                  </button>
                  <button type="button" class="btn"
                          [class.btn-warning]="selectedFilter === 'pending'"
                          [class.btn-outline-warning]="selectedFilter !== 'pending'"
                          (click)="setFilter('pending')">
                    Pending
                  </button>
                  <button type="button" class="btn"
                          [class.btn-danger]="selectedFilter === 'rejected'"
                          [class.btn-outline-danger]="selectedFilter !== 'rejected'"
                          (click)="setFilter('rejected')">
                    Rejected
                  </button>
                </div>
              </div>

              <!-- Posts List -->
              <div class="posts-list" *ngIf="!isLoading && getFilteredPosts().length > 0">
                <div class="post-card card mb-4" *ngFor="let post of getFilteredPosts()">
                  <div class="card-body">
                    <!-- Post Status Badge -->
                    <div class="d-flex justify-content-between align-items-start mb-3">
                      <span class="badge" [ngClass]="getStatusBadgeClass(post.status)">
                        <i [class]="getStatusIcon(post.status)" class="me-1"></i>
                        {{ getStatusText(post.status) }}
                      </span>
                      <small class="text-muted">{{ formatDate(post.createdAt) }}</small>
                    </div>

                    <!-- Post Content -->
                    <div class="post-content mb-3">
                      <p class="mb-2">{{ post.content }}</p>
                      <img *ngIf="post.postImagePath" [src]="post.postImagePath" 
                           class="post-image img-fluid rounded" alt="Post image">
                    </div>

                    <!-- Admin Comments (for rejected posts) -->
                    <div class="alert alert-warning" *ngIf="post.status === PostStatus.Rejected && post.adminComments">
                      <h6 class="alert-heading">
                        <i class="fas fa-comment-alt me-2"></i>Admin Feedback
                      </h6>
                      <p class="mb-0">{{ post.adminComments }}</p>
                    </div>

                    <!-- Approval Info (for approved posts) -->
                    <div class="approval-info text-muted small" 
                         *ngIf="post.status === PostStatus.Approved && post.approvedAt">
                      <i class="fas fa-check-circle text-success me-1"></i>
                      Approved {{ formatDate(post.approvedAt!) }}
                      <span *ngIf="post.approvedByUserName"> by {{ post.approvedByUserName }}</span>
                    </div>

                    <!-- Post Stats -->
                    <div class="post-stats d-flex gap-3 mt-3 pt-3 border-top" 
                         *ngIf="post.status === PostStatus.Approved">
                      <span class="text-muted">
                        <i class="fas fa-heart me-1"></i>{{ post.likesCount }} 
                        {{ post.likesCount === 1 ? 'Like' : 'Likes' }}
                      </span>
                      <span class="text-muted">
                        <i class="fas fa-comment me-1"></i>{{ post.commentsCount }} 
                        {{ post.commentsCount === 1 ? 'Comment' : 'Comments' }}
                      </span>
                    </div>
                  </div>
                </div>
              </div>

              <!-- No Posts for Current Filter -->
              <div class="text-center py-4" *ngIf="!isLoading && posts.length > 0 && getFilteredPosts().length === 0">
                <i class="fas fa-filter fa-3x text-muted mb-3"></i>
                <h5 class="text-muted">No {{ selectedFilter }} posts</h5>
                <p class="text-muted">You don't have any {{ selectedFilter }} posts yet.</p>
              </div>

              <!-- Error Message -->
              <div class="alert alert-danger" *ngIf="errorMessage">
                <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
                <button type="button" class="btn btn-sm btn-outline-danger ms-2" (click)="loadPosts()">
                  <i class="fas fa-retry me-1"></i>Retry
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .post-card {
      border: none;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .post-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.15);
    }

    .stat-card {
      border: 1px solid var(--border-color);
      transition: transform 0.2s ease;
    }

    .stat-card:hover {
      transform: translateY(-2px);
    }

    .post-image {
      max-height: 300px;
      object-fit: cover;
      width: 100%;
    }

    .badge {
      font-size: 0.875rem;
      padding: 0.5rem 0.75rem;
    }

    .btn-group .btn {
      border-radius: 0;
    }

    .btn-group .btn:first-child {
      border-top-left-radius: 0.375rem;
      border-bottom-left-radius: 0.375rem;
    }

    .btn-group .btn:last-child {
      border-top-right-radius: 0.375rem;
      border-bottom-right-radius: 0.375rem;
    }

    .approval-info {
      background-color: var(--light-bg);
      padding: 0.5rem;
      border-radius: 0.25rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .btn-group {
        display: flex;
        flex-wrap: wrap;
        gap: 0.25rem;
      }
      
      .btn-group .btn {
        border-radius: 0.375rem !important;
        flex: 1;
        min-width: 0;
      }
      
      .row.mb-4 {
        gap: 0.5rem;
      }
      
      .col-md-4 {
        flex: 1;
        min-width: 0;
      }
    }
  `]
})
export class MyPostsComponent implements OnInit {
  posts: Post[] = [];
  selectedFilter: 'all' | 'approved' | 'pending' | 'rejected' = 'all';
  isLoading = false;
  errorMessage = '';
  PostStatus = PostStatus;

  constructor(private postService: PostService) {}

  ngOnInit() {
    this.loadPosts();
  }

  loadPosts() {
    this.isLoading = true;
    this.errorMessage = '';

    this.postService.getMyPosts().subscribe({
      next: (posts) => {
        this.posts = posts;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load your posts. Please try again.';
        console.error('Error loading posts:', error);
      }
    });
  }

  setFilter(filter: 'all' | 'approved' | 'pending' | 'rejected') {
    this.selectedFilter = filter;
  }

  getFilteredPosts(): Post[] {
    if (this.selectedFilter === 'all') {
      return this.posts;
    }

    return this.posts.filter(post => {
      switch (this.selectedFilter) {
        case 'approved':
          return post.status === PostStatus.Approved;
        case 'pending':
          return post.status === PostStatus.Pending;
        case 'rejected':
          return post.status === PostStatus.Rejected;
        default:
          return true;
      }
    });
  }

  getTotalPosts(): number {
    return this.posts.length;
  }

  getApprovedPosts(): number {
    return this.posts.filter(p => p.status === PostStatus.Approved).length;
  }

  getPendingPosts(): number {
    return this.posts.filter(p => p.status === PostStatus.Pending).length;
  }

  getRejectedPosts(): number {
    return this.posts.filter(p => p.status === PostStatus.Rejected).length;
  }

  getStatusBadgeClass(status: PostStatus): string {
    switch (status) {
      case PostStatus.Approved:
        return 'bg-success';
      case PostStatus.Pending:
        return 'bg-warning';
      case PostStatus.Rejected:
        return 'bg-danger';
      default:
        return 'bg-secondary';
    }
  }

  getStatusIcon(status: PostStatus): string {
    switch (status) {
      case PostStatus.Approved:
        return 'fas fa-check-circle';
      case PostStatus.Pending:
        return 'fas fa-clock';
      case PostStatus.Rejected:
        return 'fas fa-times-circle';
      default:
        return 'fas fa-question-circle';
    }
  }

  getStatusText(status: PostStatus): string {
    switch (status) {
      case PostStatus.Approved:
        return 'Approved';
      case PostStatus.Pending:
        return 'Pending Review';
      case PostStatus.Rejected:
        return 'Rejected';
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
