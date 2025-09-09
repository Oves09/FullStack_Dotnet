import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PostService } from '../../services/post.service';
import { AuthService } from '../../services/auth.service';
import { Post, CreatePostRequest, PostComment } from '../../models/post.model';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-feed',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  template: `
    <div class="container mt-4">
      <div class="row">
        <div class="col-lg-8 mx-auto">
          <!-- Create Post Card -->
          <div class="card mb-4" *ngIf="currentUser">
            <div class="card-header">
              <h5 class="mb-0">
                <i class="fas fa-plus-circle me-2"></i>Create New Post
              </h5>
            </div>
            <div class="card-body">
              <form [formGroup]="postForm" (ngSubmit)="onCreatePost()">
                <div class="d-flex mb-3">
                  <img [src]="getUserAvatar()" class="rounded-circle me-3" width="48" height="48" alt="Your avatar">
                  <div class="flex-grow-1">
                    <textarea
                      class="form-control"
                      formControlName="content"
                      rows="3"
                      placeholder="What's on your mind, {{ currentUser?.firstName }}?"
                      [class.is-invalid]="isFieldInvalid('content')"
                    ></textarea>
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('content')">
                      Post content is required and must be less than 2000 characters
                    </div>
                  </div>
                </div>
                
                <div class="mb-3" *ngIf="selectedImage">
                  <div class="position-relative d-inline-block">
                    <img [src]="selectedImage" class="img-thumbnail" style="max-height: 200px;" alt="Selected image">
                    <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0" 
                            (click)="removeImage()" style="transform: translate(50%, -50%);">
                      <i class="fas fa-times"></i>
                    </button>
                  </div>
                </div>

                <div class="d-flex justify-content-between align-items-center">
                  <div>
                    <input type="file" #fileInput (change)="onImageSelected($event)" accept="image/*" class="d-none">
                    <button type="button" class="btn btn-outline-primary btn-sm me-2" (click)="fileInput.click()">
                      <i class="fas fa-image me-1"></i>Add Photo
                    </button>
                  </div>
                  
                  <button type="submit" class="btn btn-primary" [disabled]="postForm.invalid || isCreatingPost">
                    <span *ngIf="isCreatingPost" class="spinner-border spinner-border-sm me-2"></span>
                    <i *ngIf="!isCreatingPost" class="fas fa-paper-plane me-1"></i>
                    {{ isCreatingPost ? 'Posting...' : 'Post' }}
                  </button>
                </div>
              </form>
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

          <!-- Loading Posts -->
          <div class="text-center py-5" *ngIf="isLoadingPosts">
            <div class="spinner-border text-primary" role="status">
              <span class="visually-hidden">Loading posts...</span>
            </div>
            <p class="mt-2 text-muted">Loading posts...</p>
          </div>

          <!-- No Posts Message -->
          <div class="text-center py-5" *ngIf="!isLoadingPosts && posts.length === 0">
            <i class="fas fa-newspaper fa-3x text-muted mb-3"></i>
            <h4 class="text-muted">No posts yet</h4>
            <p class="text-muted">Be the first to share something with the community!</p>
          </div>

          <!-- Posts List -->
          <div class="post-card card" *ngFor="let post of posts">
            <div class="card-body">
              <div class="post-header">
                <img [src]="getPostUserAvatar(post)" class="user-avatar" alt="User avatar">
                <div class="user-info">
                  <h6 class="user-name">{{ post.userFullName }}</h6>
                  <p class="post-time">{{ formatDate(post.createdAt) }}</p>
                </div>
                <div class="post-actions-menu" *ngIf="isAdmin">
                  <button class="btn btn-sm btn-outline-danger" (click)="deletePost(post)" title="Delete Post">
                    <i class="fas fa-trash"></i>
                  </button>
                </div>
              </div>

              <div class="post-content">
                <p>{{ post.content }}</p>
                <img *ngIf="post.postImagePath" [src]="getFullImageUrl(post.postImagePath)" class="post-image" alt="Post image">
              </div>

              <div class="post-actions">
                <button class="action-btn" [class.liked]="post.isLikedByCurrentUser" (click)="toggleLike(post)">
                  <i class="fas fa-heart"></i>
                  {{ post.likesCount }} {{ post.likesCount === 1 ? 'Like' : 'Likes' }}
                </button>
                <button class="action-btn" (click)="toggleComments(post)">
                  <i class="fas fa-comment"></i>
                  {{ post.commentsCount }} {{ post.commentsCount === 1 ? 'Comment' : 'Comments' }}
                </button>
              </div>

              <!-- Comments Section -->
              <div class="comments-section" *ngIf="post.showComments">
                <div class="comment-form mt-3">
                  <div class="d-flex">
                    <img [src]="getUserAvatar()" class="rounded-circle me-2" width="32" height="32" alt="Your avatar">
                    <div class="flex-grow-1">
                      <div class="input-group">
                        <input 
                          type="text" 
                          class="form-control" 
                          placeholder="Write a comment..."
                          [(ngModel)]="post.newComment"
                          (keyup.enter)="addComment(post)"
                        >
                        <button class="btn btn-primary" type="button" (click)="addComment(post)" [disabled]="!post.newComment?.trim()">
                          <i class="fas fa-paper-plane"></i>
                        </button>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Comments List -->
                <div class="comments-list mt-3" *ngIf="post.comments && post.comments.length > 0">
                  <div class="comment" *ngFor="let comment of post.comments">
                    <div class="d-flex">
                      <img [src]="getCommentUserAvatar(comment)" class="rounded-circle me-2" width="32" height="32" alt="User avatar">
                      <div class="flex-grow-1">
                        <div class="comment-content">
                          <strong>{{ comment.userFullName }}</strong>
                          <span class="text-muted ms-2">{{ formatDate(comment.createdAt) }}</span>
                          <p class="mb-1">{{ comment.content }}</p>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="text-center mt-2" *ngIf="post.showComments && (!post.comments || post.comments.length === 0)">
                  <p class="text-muted">No comments yet. Be the first to comment!</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Load More Button -->
          <div class="text-center mt-4" *ngIf="!isLoadingPosts && posts.length > 0 && hasMorePosts">
            <button class="btn btn-outline-primary" (click)="loadMorePosts()" [disabled]="isLoadingMore">
              <span *ngIf="isLoadingMore" class="spinner-border spinner-border-sm me-2"></span>
              {{ isLoadingMore ? 'Loading...' : 'Load More Posts' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .post-card {
      margin-bottom: 1.5rem;
      border: none;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .post-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.15);
    }

    .post-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      margin-bottom: 1rem;
    }

    .post-actions-menu {
      margin-left: auto;
    }

    .user-avatar {
      width: 48px;
      height: 48px;
      border-radius: 50%;
      object-fit: cover;
      margin-right: 0.75rem;
    }

    .user-info .user-name {
      margin: 0;
      font-weight: 600;
      color: var(--text-primary);
    }

    .user-info .post-time {
      margin: 0;
      font-size: 0.875rem;
      color: var(--text-secondary);
    }

    .post-content {
      margin-bottom: 1rem;
      line-height: 1.6;
    }

    .post-image {
      width: 100%;
      max-height: 400px;
      object-fit: cover;
      border-radius: 0.5rem;
      margin-top: 0.75rem;
    }

    .post-actions {
      display: flex;
      gap: 1.5rem;
      padding-top: 0.75rem;
      border-top: 1px solid var(--border-color);
    }

    .action-btn {
      background: none;
      border: none;
      color: var(--text-secondary);
      font-size: 0.9rem;
      cursor: pointer;
      transition: color 0.2s ease;
      padding: 0.5rem 0;
    }

    .action-btn:hover {
      color: var(--primary-color);
    }

    .action-btn.liked {
      color: var(--danger-color);
    }

    .action-btn i {
      margin-right: 0.5rem;
    }

    .img-thumbnail {
      border: 2px solid var(--border-color);
    }

    .comments-section {
      border-top: 1px solid var(--border-color);
      padding-top: 1rem;
      margin-top: 1rem;
    }

    .comment {
      padding: 0.5rem 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .comment:last-child {
      border-bottom: none;
    }

    .comment-content {
      background-color: #f8f9fa;
      padding: 0.5rem 0.75rem;
      border-radius: 0.5rem;
      margin-top: 0.25rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .post-actions {
        gap: 1rem;
      }
      
      .action-btn {
        font-size: 0.8rem;
      }
    }
  `]
})
export class FeedComponent implements OnInit {
  postForm: FormGroup;
  posts: Post[] = [];
  currentUser: User | null = null;
  isAdmin = false;
  isLoadingPosts = false;
  isCreatingPost = false;
  isLoadingMore = false;
  hasMorePosts = true;
  currentPage = 1;
  pageSize = 10;
  selectedImage: string | null = null;
  selectedFile: File | null = null;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private postService: PostService,
    private authService: AuthService
  ) {
    this.postForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(2000)]]
    });
  }

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
      this.checkAdminRole();
    });
    this.loadPosts();
  }

  checkAdminRole() {
    if (this.currentUser) {
      // Check if user has admin role from stored user data
      this.isAdmin = this.currentUser.roles?.includes('Admin') || false;
    } else {
      this.isAdmin = false;
    }
  }

  loadPosts() {
    this.isLoadingPosts = true;
    this.postService.getPosts(1, this.pageSize).subscribe({
      next: (posts) => {
        this.posts = posts;
        this.isLoadingPosts = false;
        this.hasMorePosts = posts.length === this.pageSize;
        this.currentPage = 1;
      },
      error: (error) => {
        this.isLoadingPosts = false;
        this.errorMessage = 'Failed to load posts. Please try again.';
        console.error('Error loading posts:', error);
      }
    });
  }

  loadMorePosts() {
    this.isLoadingMore = true;
    const nextPage = this.currentPage + 1;
    
    this.postService.getPosts(nextPage, this.pageSize).subscribe({
      next: (posts) => {
        this.posts.push(...posts);
        this.isLoadingMore = false;
        this.hasMorePosts = posts.length === this.pageSize;
        this.currentPage = nextPage;
      },
      error: (error) => {
        this.isLoadingMore = false;
        this.errorMessage = 'Failed to load more posts. Please try again.';
        console.error('Error loading more posts:', error);
      }
    });
  }

  onCreatePost() {
    if (this.postForm.valid) {
      this.isCreatingPost = true;
      this.errorMessage = '';

      const createRequest: CreatePostRequest = {
        content: this.postForm.get('content')?.value,
        postImage: this.selectedFile || undefined
      };

      this.postService.createPost(createRequest).subscribe({
        next: (post) => {
          this.isCreatingPost = false;
          this.successMessage = 'Post created successfully! It will be visible after admin approval.';
          this.postForm.reset();
          this.removeImage();
        },
        error: (error) => {
          this.isCreatingPost = false;
          this.errorMessage = error.error?.message || 'Failed to create post. Please try again.';
          console.error('Error creating post:', error);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.selectedImage = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage() {
    this.selectedImage = null;
    this.selectedFile = null;
  }

  toggleLike(post: Post) {
    this.postService.likePost(post.postId).subscribe({
      next: () => {
        post.isLikedByCurrentUser = !post.isLikedByCurrentUser;
        post.likesCount += post.isLikedByCurrentUser ? 1 : -1;
      },
      error: (error) => {
        this.errorMessage = 'Failed to update like. Please try again.';
        console.error('Error toggling like:', error);
      }
    });
  }

  getUserAvatar(): string {
    if (this.currentUser?.profileImagePath) {
      return this.getFullImageUrl(this.currentUser.profileImagePath);
    }
    return 'https://via.placeholder.com/48x48/3b82f6/ffffff?text=' + 
           (this.currentUser?.firstName?.charAt(0) || this.currentUser?.userName?.charAt(0) || 'U');
  }

  getPostUserAvatar(post: Post): string {
    if (post.userProfileImage) {
      return this.getFullImageUrl(post.userProfileImage);
    }
    return 'https://via.placeholder.com/48x48/3b82f6/ffffff?text=' + 
           (post.userFullName?.charAt(0) || 'U');
  }

  getFullImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    if (imagePath.startsWith('http')) return imagePath;
    return `http://localhost:5120${imagePath}`;
  }

  formatDate(date: Date): string {
    const now = new Date();
    const postDate = new Date(date);
    const diffInMinutes = Math.floor((now.getTime() - postDate.getTime()) / (1000 * 60));

    if (diffInMinutes < 1) return 'Just now';
    if (diffInMinutes < 60) return `${diffInMinutes}m ago`;
    if (diffInMinutes < 1440) return `${Math.floor(diffInMinutes / 60)}h ago`;
    if (diffInMinutes < 10080) return `${Math.floor(diffInMinutes / 1440)}d ago`;
    
    return postDate.toLocaleDateString();
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.postForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  toggleComments(post: Post) {
    post.showComments = !post.showComments;
    if (post.showComments && !post.comments) {
      this.loadComments(post);
    }
  }

  loadComments(post: Post) {
    this.postService.getComments(post.postId).subscribe({
      next: (comments: PostComment[]) => {
        post.comments = comments;
      },
      error: (error: any) => {
        this.errorMessage = 'Failed to load comments. Please try again.';
        console.error('Error loading comments:', error);
      }
    });
  }

  addComment(post: Post) {
    if (!post.newComment?.trim()) return;

    const commentContent = post.newComment.trim();
    post.newComment = '';

    this.postService.addComment(post.postId, commentContent).subscribe({
      next: (comment: PostComment) => {
        if (!post.comments) {
          post.comments = [];
        }
        post.comments.unshift(comment);
        post.commentsCount++;
      },
      error: (error: any) => {
        this.errorMessage = 'Failed to add comment. Please try again.';
        console.error('Error adding comment:', error);
        post.newComment = commentContent; // Restore the comment text
      }
    });
  }

  getCommentUserAvatar(comment: any): string {
    if (comment.userProfileImage) {
      return this.getFullImageUrl(comment.userProfileImage);
    }
    return 'https://via.placeholder.com/32x32/3b82f6/ffffff?text=' + 
           (comment.userFullName?.charAt(0) || 'U');
  }

  deletePost(post: Post) {
    if (!confirm('Are you sure you want to permanently delete this post? This action cannot be undone.')) {
      return;
    }

    this.postService.deletePost(post.postId).subscribe({
      next: () => {
        this.successMessage = 'Post deleted successfully!';
        // Remove the post from the list
        this.posts = this.posts.filter(p => p.postId !== post.postId);
      },
      error: (error: any) => {
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

  private markFormGroupTouched() {
    Object.keys(this.postForm.controls).forEach(key => {
      const control = this.postForm.get(key);
      control?.markAsTouched();
    });
  }
}
