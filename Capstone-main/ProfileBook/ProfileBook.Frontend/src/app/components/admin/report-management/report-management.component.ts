import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';
import { Report, ReportStatus, ReviewReportRequest } from '../../../models/report.model';

@Component({
  selector: 'app-report-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-12">
          <div class="card shadow-sm">
            <div class="card-header bg-warning text-dark">
              <h4 class="mb-0">
                <i class="fas fa-flag me-2"></i>Report Management
              </h4>
            </div>
            <div class="card-body">
              <!-- Filter Tabs -->
              <ul class="nav nav-tabs mb-4">
                <li class="nav-item">
                  <button class="nav-link" 
                          [class.active]="selectedStatus === ReportStatus.Pending"
                          (click)="filterByStatus(ReportStatus.Pending)">
                    <i class="fas fa-clock me-1"></i>
                    Pending ({{ getStatusCount(ReportStatus.Pending) }})
                  </button>
                </li>
                <li class="nav-item">
                  <button class="nav-link" 
                          [class.active]="selectedStatus === ReportStatus.UnderReview"
                          (click)="filterByStatus(ReportStatus.UnderReview)">
                    <i class="fas fa-search me-1"></i>
                    Under Review ({{ getStatusCount(ReportStatus.UnderReview) }})
                  </button>
                </li>
                <li class="nav-item">
                  <button class="nav-link" 
                          [class.active]="selectedStatus === ReportStatus.Resolved"
                          (click)="filterByStatus(ReportStatus.Resolved)">
                    <i class="fas fa-check-circle me-1"></i>
                    Resolved ({{ getStatusCount(ReportStatus.Resolved) }})
                  </button>
                </li>
                <li class="nav-item">
                  <button class="nav-link" 
                          [class.active]="selectedStatus === ReportStatus.Dismissed"
                          (click)="filterByStatus(ReportStatus.Dismissed)">
                    <i class="fas fa-times-circle me-1"></i>
                    Dismissed ({{ getStatusCount(ReportStatus.Dismissed) }})
                  </button>
                </li>
                <li class="nav-item">
                  <button class="nav-link" 
                          [class.active]="selectedStatus === null"
                          (click)="filterByStatus(null)">
                    <i class="fas fa-list me-1"></i>
                    All Reports ({{ allReports.length }})
                  </button>
                </li>
              </ul>

              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-warning" role="status">
                  <span class="visually-hidden">Loading reports...</span>
                </div>
                <p class="mt-2 text-muted">Loading reports...</p>
              </div>

              <!-- No Reports -->
              <div class="text-center py-5" *ngIf="!isLoading && filteredReports.length === 0">
                <i class="fas fa-shield-alt fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No reports found</h5>
                <p class="text-muted">
                  {{ selectedStatus !== null ? 'No reports with this status.' : 'No reports have been submitted yet.' }}
                </p>
                <button class="btn btn-warning" (click)="loadReports()">
                  <i class="fas fa-sync-alt me-2"></i>Refresh
                </button>
              </div>

              <!-- Reports List -->
              <div class="reports-list" *ngIf="!isLoading && filteredReports.length > 0">
                <div class="report-card card mb-4" *ngFor="let report of paginatedReports; let i = index">
                  <div class="card-body">
                    <!-- Report Header -->
                    <div class="row mb-3">
                      <div class="col-md-8">
                        <div class="d-flex align-items-start">
                          <div class="flex-grow-1">
                            <h6 class="mb-1">
                              Report against <strong>{{ report.reportedUserFullName }}</strong>
                              <small class="text-muted">({{ '@' + report.reportedUserName }})</small>
                            </h6>
                            <p class="mb-1 text-muted">
                              Reported by <strong>{{ report.reportingUserFullName }}</strong>
                              <small>({{ '@' + report.reportingUserName }})</small>
                            </p>
                            <small class="text-muted">{{ formatDate(report.timeStamp) }}</small>
                          </div>
                        </div>
                      </div>
                      <div class="col-md-4 text-end">
                        <span class="badge" [ngClass]="getStatusBadgeClass(report.status)">
                          <i [class]="getStatusIcon(report.status)" class="me-1"></i>
                          {{ getStatusText(report.status) }}
                        </span>
                      </div>
                    </div>

                    <!-- Report Details -->
                    <div class="report-details mb-3">
                      <div class="row">
                        <div class="col-md-6">
                          <div class="mb-2">
                            <strong>Reason:</strong>
                            <span class="ms-2 badge bg-secondary">{{ report.reason }}</span>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-2">
                            <strong>Report ID:</strong>
                            <span class="ms-2 font-monospace">#{{ report.reportId }}</span>
                          </div>
                        </div>
                      </div>
                      
                      <div *ngIf="report.description" class="mb-2">
                        <strong>Description:</strong>
                        <p class="mb-0 mt-1 p-2 bg-light rounded">{{ report.description }}</p>
                      </div>

                      <!-- Previous Admin Response -->
                      <div *ngIf="report.adminNotes && report.status !== ReportStatus.Pending" class="mb-3">
                        <div class="alert" [ngClass]="getAdminResponseClass(report.status)">
                          <h6 class="alert-heading">
                            <i class="fas fa-user-shield me-2"></i>Previous Admin Response
                          </h6>
                          <p class="mb-1">{{ report.adminNotes }}</p>
                          <small class="text-muted">
                            Reviewed {{ formatDate(report.reviewedAt!) }}
                            <span *ngIf="report.reviewedByUserName"> by {{ report.reviewedByUserName }}</span>
                          </small>
                        </div>
                      </div>
                    </div>

                    <!-- Action Section -->
                    <div class="action-section border-top pt-3" 
                         *ngIf="report.status === ReportStatus.Pending || report.status === ReportStatus.UnderReview">
                      <form [formGroup]="getReviewForm(i)" (ngSubmit)="reviewReport(report, i, ReportStatus.Resolved)">
                        <div class="mb-3">
                          <label class="form-label">Admin Notes <span class="text-danger">*</span></label>
                          <textarea
                            class="form-control"
                            formControlName="adminNotes"
                            rows="3"
                            placeholder="Provide details about your decision and any actions taken..."
                            [class.is-invalid]="getReviewForm(i).get('adminNotes')?.invalid && getReviewForm(i).get('adminNotes')?.touched"
                          ></textarea>
                          <div class="invalid-feedback">
                            Admin notes are required when reviewing a report.
                          </div>
                        </div>

                        <div class="d-flex gap-2">
                          <button type="submit" class="btn btn-success" 
                                  [disabled]="isProcessing[i] || getReviewForm(i).invalid">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm me-2"></span>
                            <i *ngIf="!isProcessing[i]" class="fas fa-check me-1"></i>
                            {{ isProcessing[i] ? 'Resolving...' : 'Mark as Resolved' }}
                          </button>
                          
                          <button type="button" class="btn btn-secondary" 
                                  (click)="reviewReport(report, i, ReportStatus.Dismissed)"
                                  [disabled]="isProcessing[i] || getReviewForm(i).invalid">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm me-2"></span>
                            <i *ngIf="!isProcessing[i]" class="fas fa-times me-1"></i>
                            {{ isProcessing[i] ? 'Dismissing...' : 'Dismiss Report' }}
                          </button>

                          <button type="button" class="btn btn-info" 
                                  (click)="markUnderReview(report, i)"
                                  [disabled]="isProcessing[i] || report.status === ReportStatus.UnderReview">
                            <i class="fas fa-search me-1"></i>
                            Mark Under Review
                          </button>
                        </div>
                      </form>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Pagination -->
              <nav *ngIf="!isLoading && filteredReports.length > pageSize">
                <ul class="pagination justify-content-center">
                  <li class="page-item" [class.disabled]="currentPage === 1">
                    <button class="page-link" (click)="changePage(currentPage - 1)">Previous</button>
                  </li>
                  <li class="page-item" *ngFor="let page of getPageNumbers()" 
                      [class.active]="page === currentPage">
                    <button class="page-link" (click)="changePage(page)">{{ page }}</button>
                  </li>
                  <li class="page-item" [class.disabled]="currentPage === totalPages">
                    <button class="page-link" (click)="changePage(currentPage + 1)">Next</button>
                  </li>
                </ul>
              </nav>

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
    .report-card {
      border: none;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .report-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.15);
    }

    .nav-tabs .nav-link {
      border: none;
      color: var(--text-muted);
      background: transparent;
    }

    .nav-tabs .nav-link.active {
      background-color: var(--warning);
      color: var(--dark);
      border-color: var(--warning);
    }

    .nav-tabs .nav-link:hover {
      border-color: transparent;
      background-color: var(--light);
    }

    .action-section {
      background-color: var(--light-bg);
      margin: -1rem -1rem 0 -1rem;
      padding: 1rem;
      border-radius: 0 0 0.75rem 0.75rem;
    }

    .badge {
      font-size: 0.875rem;
      padding: 0.5rem 0.75rem;
    }

    .font-monospace {
      font-family: 'Courier New', monospace;
      font-size: 0.875rem;
    }

    .alert-heading {
      font-size: 0.9rem;
      margin-bottom: 0.5rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .nav-tabs {
        flex-wrap: wrap;
      }
      
      .nav-tabs .nav-link {
        font-size: 0.875rem;
        padding: 0.5rem 0.75rem;
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
export class ReportManagementComponent implements OnInit {
  allReports: Report[] = [];
  filteredReports: Report[] = [];
  paginatedReports: Report[] = [];
  reviewForms: FormGroup[] = [];
  isLoading = false;
  isProcessing: boolean[] = [];
  successMessage = '';
  errorMessage = '';
  selectedStatus: ReportStatus | null = ReportStatus.Pending;

  // Pagination
  currentPage = 1;
  pageSize = 5;
  totalPages = 1;

  ReportStatus = ReportStatus;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService
  ) {}

  ngOnInit() {
    this.loadReports();
  }

  loadReports() {
    this.isLoading = true;
    this.errorMessage = '';

    // Load all reports
    this.adminService.getAllReports().subscribe({
      next: (reports) => {
        this.allReports = reports;
        this.filterByStatus(this.selectedStatus);
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load reports. Please try again.';
        console.error('Error loading reports:', error);
      }
    });
  }

  filterByStatus(status: ReportStatus | null) {
    this.selectedStatus = status;
    
    if (status === null) {
      this.filteredReports = [...this.allReports];
    } else {
      this.filteredReports = this.allReports.filter(report => report.status === status);
    }

    this.currentPage = 1;
    this.initializeForms();
    this.updatePagination();
  }

  initializeForms() {
    this.reviewForms = [];
    this.isProcessing = [];
    
    this.filteredReports.forEach(() => {
      this.reviewForms.push(this.fb.group({
        adminNotes: ['', Validators.required]
      }));
      this.isProcessing.push(false);
    });
  }

  getReviewForm(index: number): FormGroup {
    return this.reviewForms[index];
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredReports.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedReports = this.filteredReports.slice(startIndex, endIndex);
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  getPageNumbers(): number[] {
    const pages = [];
    const maxPages = 5;
    let start = Math.max(1, this.currentPage - Math.floor(maxPages / 2));
    let end = Math.min(this.totalPages, start + maxPages - 1);
    
    if (end - start < maxPages - 1) {
      start = Math.max(1, end - maxPages + 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }

  reviewReport(report: Report, index: number, newStatus: ReportStatus) {
    const form = this.getReviewForm(index);
    if (form.invalid) {
      form.markAllAsTouched();
      return;
    }

    this.isProcessing[index] = true;
    this.errorMessage = '';
    this.successMessage = '';

    const reviewRequest: ReviewReportRequest = {
      reportId: report.reportId,
      status: newStatus,
      adminNotes: form.get('adminNotes')?.value
    };

    this.adminService.reviewReport(reviewRequest).subscribe({
      next: () => {
        this.isProcessing[index] = false;
        this.successMessage = `Report ${newStatus === ReportStatus.Resolved ? 'resolved' : 'dismissed'} successfully!`;
        
        // Update the report in the local array
        const reportIndex = this.allReports.findIndex(r => r.reportId === report.reportId);
        if (reportIndex > -1) {
          this.allReports[reportIndex].status = newStatus;
          this.allReports[reportIndex].adminNotes = reviewRequest.adminNotes;
          this.allReports[reportIndex].reviewedAt = new Date();
        }

        // Refresh the filtered view
        this.filterByStatus(this.selectedStatus);
      },
      error: (error) => {
        this.isProcessing[index] = false;
        this.errorMessage = error.error?.message || 'Failed to review report. Please try again.';
        console.error('Error reviewing report:', error);
      }
    });
  }

  markUnderReview(report: Report, index: number) {
    this.reviewReport(report, index, ReportStatus.UnderReview);
  }

  getStatusCount(status: ReportStatus): number {
    return this.allReports.filter(report => report.status === status).length;
  }

  getStatusBadgeClass(status: ReportStatus): string {
    switch (status) {
      case ReportStatus.Pending:
        return 'bg-warning';
      case ReportStatus.UnderReview:
        return 'bg-info';
      case ReportStatus.Resolved:
        return 'bg-success';
      case ReportStatus.Dismissed:
        return 'bg-secondary';
      default:
        return 'bg-secondary';
    }
  }

  getStatusIcon(status: ReportStatus): string {
    switch (status) {
      case ReportStatus.Pending:
        return 'fas fa-clock';
      case ReportStatus.UnderReview:
        return 'fas fa-search';
      case ReportStatus.Resolved:
        return 'fas fa-check-circle';
      case ReportStatus.Dismissed:
        return 'fas fa-times-circle';
      default:
        return 'fas fa-question-circle';
    }
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

  getAdminResponseClass(status: ReportStatus): string {
    switch (status) {
      case ReportStatus.Resolved:
        return 'alert-success';
      case ReportStatus.Dismissed:
        return 'alert-secondary';
      default:
        return 'alert-info';
    }
  }

  formatDate(date: Date): string {
    const reportDate = new Date(date);
    const now = new Date();
    const diffInMinutes = Math.floor((now.getTime() - reportDate.getTime()) / (1000 * 60));

    if (diffInMinutes < 60) return `${diffInMinutes}m ago`;
    if (diffInMinutes < 1440) return `${Math.floor(diffInMinutes / 60)}h ago`;
    if (diffInMinutes < 10080) return `${Math.floor(diffInMinutes / 1440)}d ago`;
    
    return reportDate.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  }
}
