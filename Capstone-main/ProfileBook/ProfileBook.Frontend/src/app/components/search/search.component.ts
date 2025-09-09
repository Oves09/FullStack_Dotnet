import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ReportService } from '../../services/report.service';
import { User } from '../../models/user.model';
import { CreateReportRequest } from '../../models/report.model';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';
import { of } from 'rxjs';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <!-- Search Header -->
          <div class="card shadow-sm mb-4">
            <div class="card-body">
              <h4 class="card-title mb-3">
                <i class="fas fa-search me-2"></i>Search Users
              </h4>
              
              <form [formGroup]="searchForm">
                <div class="input-group">
                  <span class="input-group-text">
                    <i class="fas fa-search"></i>
                  </span>
                  <input
                    type="text"
                    class="form-control"
                    formControlName="query"
                    placeholder="Search by name, username, or email..."
                  >
                </div>
              </form>
            </div>
          </div>

          <!-- Search Results -->
          <div class="card shadow-sm" *ngIf="searchQuery">
            <div class="card-header">
              <h5 class="mb-0">
                Search Results for "{{ searchQuery }}"
                <span class="badge bg-primary ms-2" *ngIf="users.length > 0">{{ users.length }}</span>
              </h5>
            </div>
            <div class="card-body">
              <!-- Loading State -->
              <div class="text-center py-4" *ngIf="isSearching">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Searching...</span>
                </div>
                <p class="mt-2 text-muted">Searching users...</p>
              </div>

              <!-- No Results -->
              <div class="text-center py-4" *ngIf="!isSearching && users.length === 0 && searchQuery">
                <i class="fas fa-user-slash fa-3x text-muted mb-3"></i>
                <h5 class="text-muted">No users found</h5>
                <p class="text-muted">Try adjusting your search terms</p>
              </div>

              <!-- User Results -->
              <div class="row" *ngIf="!isSearching && users.length > 0">
                <div class="col-md-6 mb-3" *ngFor="let user of users">
                  <div class="card user-card h-100">
                    <div class="card-body">
                      <div class="d-flex align-items-center mb-3">
                        <img [src]="getUserAvatar(user)" class="rounded-circle me-3" 
                             width="60" height="60" alt="User avatar">
                        <div class="flex-grow-1">
                          <h6 class="card-title mb-1">{{ user.firstName }} {{ user.lastName }}</h6>
                          <p class="text-muted mb-1">{{ '@' + user.userName }}</p>
                          <small class="text-muted">{{ user.email }}</small>
                        </div>
                      </div>

                      <p class="card-text" *ngIf="user.bio">
                        {{ user.bio | slice:0:100 }}{{ user.bio.length > 100 ? '...' : '' }}
                      </p>

                      <div class="d-flex flex-wrap gap-1 mb-3" *ngIf="user.roles && user.roles.length > 0">
                        <span *ngFor="let role of user.roles" 
                              class="badge bg-secondary rounded-pill">
                          {{ role }}
                        </span>
                      </div>

                      <div class="d-flex gap-2">
                        <button class="btn btn-primary btn-sm" (click)="sendMessage(user.id)">
                          <i class="fas fa-envelope me-1"></i>Message
                        </button>
                        <button class="btn btn-outline-danger btn-sm" (click)="openReportModal(user)">
                          <i class="fas fa-flag me-1"></i>Report
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Load More Button -->
              <div class="text-center mt-4" *ngIf="!isSearching && users.length > 0 && hasMoreResults">
                <button class="btn btn-outline-primary" (click)="loadMoreResults()" [disabled]="isLoadingMore">
                  <span *ngIf="isLoadingMore" class="spinner-border spinner-border-sm me-2"></span>
                  {{ isLoadingMore ? 'Loading...' : 'Load More' }}
                </button>
              </div>
            </div>
          </div>

          <!-- Initial State -->
          <div class="card shadow-sm" *ngIf="!searchQuery">
            <div class="card-body text-center py-5">
              <i class="fas fa-users fa-4x text-muted mb-3"></i>
              <h4 class="text-muted">Find People</h4>
              <p class="text-muted">Search for users by name, username, or email to connect with them</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Report Modal -->
    <div class="modal fade" id="reportModal" tabindex="-1" *ngIf="selectedUser">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="fas fa-flag me-2"></i>Report User
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <p class="mb-3">
              You are reporting <strong>{{ selectedUser?.firstName }} {{ selectedUser?.lastName }}</strong>
              ({{ '@' + selectedUser?.userName }})
            </p>

            <form [formGroup]="reportForm" (ngSubmit)="submitReport()">
              <div class="mb-3">
                <label for="reason" class="form-label">Reason for Report</label>
                <select class="form-select" id="reason" formControlName="reason" 
                        [class.is-invalid]="isReportFieldInvalid('reason')">
                  <option value="">Select a reason...</option>
                  <option value="Harassment">Harassment</option>
                  <option value="Spam">Spam</option>
                  <option value="Inappropriate Content">Inappropriate Content</option>
                  <option value="Fake Profile">Fake Profile</option>
                  <option value="Other">Other</option>
                </select>
                <div class="invalid-feedback" *ngIf="isReportFieldInvalid('reason')">
                  Please select a reason for reporting
                </div>
              </div>

              <div class="mb-3">
                <label for="description" class="form-label">Additional Details (Optional)</label>
                <textarea
                  class="form-control"
                  id="description"
                  formControlName="description"
                  rows="3"
                  placeholder="Provide additional details about the issue..."
                ></textarea>
              </div>

              <div class="alert alert-warning">
                <i class="fas fa-exclamation-triangle me-2"></i>
                <small>Reports are reviewed by our moderation team. False reports may result in action against your account.</small>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
            <button type="button" class="btn btn-danger" (click)="submitReport()" 
                    [disabled]="reportForm.invalid || isSubmittingReport">
              <span *ngIf="isSubmittingReport" class="spinner-border spinner-border-sm me-2"></span>
              {{ isSubmittingReport ? 'Submitting...' : 'Submit Report' }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Success/Error Messages -->
    <div class="position-fixed top-0 end-0 p-3" style="z-index: 1050;">
      <div class="toast show" *ngIf="successMessage" role="alert">
        <div class="toast-header bg-success text-white">
          <i class="fas fa-check-circle me-2"></i>
          <strong class="me-auto">Success</strong>
          <button type="button" class="btn-close btn-close-white" (click)="successMessage = ''"></button>
        </div>
        <div class="toast-body">{{ successMessage }}</div>
      </div>

      <div class="toast show" *ngIf="errorMessage" role="alert">
        <div class="toast-header bg-danger text-white">
          <i class="fas fa-exclamation-triangle me-2"></i>
          <strong class="me-auto">Error</strong>
          <button type="button" class="btn-close btn-close-white" (click)="errorMessage = ''"></button>
        </div>
        <div class="toast-body">{{ errorMessage }}</div>
      </div>
    </div>
  `,
  styles: [`
    .user-card {
      transition: transform 0.2s ease, box-shadow 0.2s ease;
      border: none;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .user-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.15);
    }

    .input-group-text {
      background-color: var(--light-bg);
      border-color: var(--border-color);
    }

    .badge {
      font-size: 0.75rem;
    }

    .modal-content {
      border: none;
      border-radius: 0.75rem;
    }

    .modal-header {
      border-bottom: 1px solid var(--border-color);
    }

    .toast {
      min-width: 300px;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .user-card .d-flex.gap-2 {
        flex-direction: column;
      }
      
      .user-card .btn {
        margin-bottom: 0.25rem;
      }
    }
  `]
})
export class SearchComponent implements OnInit {
  searchForm: FormGroup;
  reportForm: FormGroup;
  users: User[] = [];
  selectedUser: User | null = null;
  searchQuery = '';
  isSearching = false;
  isLoadingMore = false;
  isSubmittingReport = false;
  hasMoreResults = false;
  currentPage = 1;
  pageSize = 10;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private reportService: ReportService,
    private router: Router
  ) {
    this.searchForm = this.fb.group({
      query: ['']
    });

    this.reportForm = this.fb.group({
      reason: ['', [Validators.required]],
      description: ['']
    });
  }

  ngOnInit() {
    // Setup search with debouncing
    this.searchForm.get('query')?.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(query => {
          this.searchQuery = query?.trim() || '';
          if (this.searchQuery.length >= 2) {
            this.isSearching = true;
            this.currentPage = 1;
            return this.userService.searchUsers(this.searchQuery, 1, this.pageSize);
          } else {
            this.users = [];
            return of([]);
          }
        })
      )
      .subscribe({
        next: (users) => {
          this.users = users;
          this.isSearching = false;
          this.hasMoreResults = users.length === this.pageSize;
        },
        error: (error) => {
          this.isSearching = false;
          this.errorMessage = 'Failed to search users. Please try again.';
          console.error('Search error:', error);
        }
      });
  }

  loadMoreResults() {
    if (this.searchQuery && !this.isLoadingMore) {
      this.isLoadingMore = true;
      const nextPage = this.currentPage + 1;

      this.userService.searchUsers(this.searchQuery, nextPage, this.pageSize).subscribe({
        next: (users) => {
          this.users.push(...users);
          this.isLoadingMore = false;
          this.hasMoreResults = users.length === this.pageSize;
          this.currentPage = nextPage;
        },
        error: (error) => {
          this.isLoadingMore = false;
          this.errorMessage = 'Failed to load more results. Please try again.';
          console.error('Load more error:', error);
        }
      });
    }
  }

  sendMessage(userId: string) {
    this.router.navigate(['/messages', userId]);
  }

  openReportModal(user: User) {
    this.selectedUser = user;
    this.reportForm.reset();
    
    // Show modal using Bootstrap
    const modal = new (window as any).bootstrap.Modal(document.getElementById('reportModal'));
    modal.show();
  }

  submitReport() {
    if (this.reportForm.valid && this.selectedUser) {
      this.isSubmittingReport = true;
      this.errorMessage = '';

      const reportRequest: CreateReportRequest = {
        reportedUserId: this.selectedUser.id,
        reason: this.reportForm.get('reason')?.value,
        description: this.reportForm.get('description')?.value || undefined
      };

      this.reportService.createReport(reportRequest).subscribe({
        next: () => {
          this.isSubmittingReport = false;
          this.successMessage = 'Report submitted successfully. Our team will review it.';
          
          // Hide modal
          const modal = (window as any).bootstrap.Modal.getInstance(document.getElementById('reportModal'));
          modal?.hide();
          
          this.selectedUser = null;
          this.reportForm.reset();
        },
        error: (error) => {
          this.isSubmittingReport = false;
          this.errorMessage = error.error?.message || 'Failed to submit report. Please try again.';
          console.error('Report error:', error);
        }
      });
    } else {
      this.markReportFormTouched();
    }
  }

  getUserAvatar(user: User): string {
    if (user.profileImagePath) {
      return this.getFullImageUrl(user.profileImagePath);
    }
    return 'https://via.placeholder.com/60x60/3b82f6/ffffff?text=' + 
           (user.firstName?.charAt(0) || user.userName?.charAt(0) || 'U');
  }

  getFullImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    if (imagePath.startsWith('http')) return imagePath;
    return `http://localhost:5120${imagePath}`;
  }

  isReportFieldInvalid(fieldName: string): boolean {
    const field = this.reportForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  private markReportFormTouched() {
    Object.keys(this.reportForm.controls).forEach(key => {
      const control = this.reportForm.get(key);
      control?.markAsTouched();
    });
  }
}
