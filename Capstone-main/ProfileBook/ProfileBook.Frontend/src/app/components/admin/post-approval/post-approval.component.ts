import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';
import { PostService } from '../../../services/post.service';
import { Post, PostStatus, ApprovePostRequest } from '../../../models/post.model';

@Component({
  selector: 'app-post-approval',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-10">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">
                <i class="fas fa-file-alt me-2"></i>Post Approval Management
              </h4>
            </div>
            <div class="card-body">
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading pending posts...</span>
                </div>
                <p class="mt-2 text-muted">Loading posts for review...</p>
              </div>

              <!-- No Pending Posts -->
              <div class="text-center py-5" *ngIf="!isLoading && posts.length === 0">
                <i class="fas fa-check-circle fa-4x text-success mb-3"></i>
                <h5 class="text-muted">All caught up!</h5>
                <p class="text-muted">There are no posts pending approval at the moment.</p>
                <button class="btn btn-primary" (click)="loadPosts()">
                  <i class="fas fa-sync-alt me-2"></i>Refresh
                </button>
              </div>

              <!-- Posts List -->
              <div class="posts-list" *ngIf="!isLoading && posts.length > 0">
                <div class="post-card card mb-4" *ngFor="let post of posts; let i = index">
                  <div class="card-body">
                    <!-- Post Header -->
                    <div class="d-flex align-items-center mb-3">
                      <img [src]="getPostUserAvatar(post)" class="rounded-circle me-3" 
                           width="48" height="48" alt="User avatar">
                      <div class="flex-grow-1">
                        <h6 class="mb-1">{{ post.userFullName }}</h6>
                        <small class="text-muted">{{ '@' + post.userName }} • {{ formatDate(post.createdAt) }}</small>
                      </div>
                      <span class="badge bg-warning">
                        <i class="fas fa-clock me-1"></i>Pending Review
                      </span>
                    </div>

                    <!-- Post Content -->
                    <div class="post-content mb-3">
                      <p class="mb-2">{{ post.content }}</p>
                      <img *ngIf="post.postImagePath" [src]="post.postImagePath" 
                           class="post-image img-fluid rounded" alt="Post image">
                    </div>

                    <!-- Approval Form -->
                    <div class="approval-section border-top pt-3">
                      <form [formGroup]="getApprovalForm(i)" (ngSubmit)="approvePost(post, i, true)">
                        <div class="mb-3">
                          <label class="form-label">Admin Comments (Optional)</label>
                          <textarea
                            class="form-control"
                            formControlName="adminComments"
                            rows="2"
                            placeholder="Add any comments for the user..."
                          ></textarea>
                        </div>

                        <div class="d-flex gap-2">
                          <button type="submit" class="btn btn-success" 
                                  [disabled]="isProcessing[i]">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm me-2"></span>
                            <i *ngIf="!isProcessing[i]" class="fas fa-check me-1"></i>
                            {{ isProcessing[i] ? 'Approving...' : 'Approve' }}
                          </button>
                          
                          <button type="button" class="btn btn-danger" 
                                  (click)="approvePost(post, i, false)"
                                  [disabled]="isProcessing[i]">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm me-2"></span>
                            <i *ngIf="!isProcessing[i]" class="fas fa-times me-1"></i>
                            {{ isProcessing[i] ? 'Rejecting...' : 'Reject' }}
                          </button>
                          
                          <button type="button" class="btn btn-outline-danger" 
                                  (click)="deletePost(post, i)"
                                  [disabled]="isProcessing[i]">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm me-2"></span>
                            <i *ngIf="!isProcessing[i]" class="fas fa-trash me-1"></i>
                            {{ isProcessing[i] ? 'Deleting...' : 'Delete' }}
                          </button>
                        </div>
                      </form>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Success Message -->
              <div class="alert alert-success alert-dismissible fade show" *ngIf="successMessage">
                <i class="fas fa-check-circle me-2"></i>{{ successMessage }}
                <button type="button" class="btn-close" (click)="successMessage = ''"></button>
              </div>

              <!-- Error Message -->
              <div class="alert alert-danger alert-dismissible fade show" *ngIf="errorMessage">
                <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
                <button type="button" class="btn-close" (click)="errorMessage = ''"></button>
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

    .post-image {
      max-height: 300px;
      object-fit: cover;
      width: 100%;
    }

    .approval-section {
      background-color: var(--light-bg);
      margin: -1rem -1rem 0 -1rem;
      padding: 1rem;
      border-radius: 0 0 0.75rem 0.75rem;
    }

    .badge {
      font-size: 0.875rem;
      padding: 0.5rem 0.75rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .d-flex.gap-2 {
        flex-direction: column;
      }
      
      .d-flex.gap-2 .btn {
        margin-bottom: 0.5rem;
      }
    }
  `]
})
export class PostApprovalComponent implements OnInit {
  posts: Post[] = [];
  approvalForms: FormGroup[] = [];
  isLoading = false;
  isProcessing: boolean[] = [];
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private postService: PostService
  ) {}

  ngOnInit() {
    this.loadPosts();
  }

  loadPosts() {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getPendingPosts().subscribe({
      next: (posts) => {
        this.posts = posts;
        this.initializeForms();
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        if (error.status === 401) {
          this.errorMessage = 'Authentication failed. Please log in as an admin.';
        } else if (error.status === 403) {
          this.errorMessage = 'Access denied. Admin privileges required.';
        } else {
          this.errorMessage = 'Failed to load posts. Please try again.';
        }
        console.error('Error loading posts:', error);
      }
    });
  }

  initializeForms() {
    this.approvalForms = [];
    this.isProcessing = [];
    
    this.posts.forEach(() => {
      this.approvalForms.push(this.fb.group({
        adminComments: ['']
      }));
      this.isProcessing.push(false);
    });
  }

  getApprovalForm(index: number): FormGroup {
    return this.approvalForms[index];
  }

  approvePost(post: Post, index: number, isApproved: boolean) {
    this.isProcessing[index] = true;
    this.errorMessage = '';
    this.successMessage = '';

    const approvalRequest: ApprovePostRequest = {
      postId: post.postId,
      isApproved: isApproved,
      adminComments: this.approvalForms[index].get('adminComments')?.value || undefined
    };

    this.adminService.approvePost(approvalRequest).subscribe({
      next: () => {
        this.isProcessing[index] = false;
        this.successMessage = `Post ${isApproved ? 'approved' : 'rejected'} successfully!`;
        
        // Remove the post from the list
        this.posts.splice(index, 1);
        this.approvalForms.splice(index, 1);
        this.isProcessing.splice(index, 1);
      },
      error: (error) => {
        this.isProcessing[index] = false;
        if (error.status === 401) {
          this.errorMessage = 'Authentication failed. Please log in as an admin.';
        } else if (error.status === 403) {
          this.errorMessage = 'Access denied. Admin privileges required.';
        } else {
          this.errorMessage = error.error?.message || `Failed to ${isApproved ? 'approve' : 'reject'} post. Please try again.`;
        }
        console.error('Error processing post:', error);
      }
    });
  }

  getPostUserAvatar(post: Post): string {
    if (post.userProfileImage) {
      return post.userProfileImage;
    }
    return 'https://via.placeholder.com/48x48/3b82f6/ffffff?text=' + 
           (post.userFullName?.charAt(0) || 'U');
  }

  deletePost(post: Post, index: number) {
    if (!confirm('Are you sure you want to permanently delete this post? This action cannot be undone.')) {
      return;
    }

    this.isProcessing[index] = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.postService.deletePost(post.postId).subscribe({
      next: () => {
        this.isProcessing[index] = false;
        this.successMessage = 'Post deleted successfully!';
        
        // Remove the post from the list
        this.posts.splice(index, 1);
        this.approvalForms.splice(index, 1);
        this.isProcessing.splice(index, 1);
      },
      error: (error) => {
        this.isProcessing[index] = false;
        if (error.status === 401) {
          this.errorMessage = 'Authentication failed. Please log in as an admin.';
        } else if (error.status === 403) {
          this.errorMessage = 'Access denied. Admin privileges required.';
        } else {
          this.errorMessage = error.error?.message || 'Failed to delete post. Please try again.';
        }
        console.error('Error deleting post:', error);
      }
    });
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
