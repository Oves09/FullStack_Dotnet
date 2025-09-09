import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportService } from '../../services/report.service';
import { Report, ReportStatus } from '../../models/report.model';

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-10">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">
                <i class="fas fa-flag me-2"></i>My Reports
              </h4>
            </div>
            <div class="card-body">
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading your reports...</span>
                </div>
                <p class="mt-2 text-muted">Loading your reports...</p>
              </div>

              <!-- No Reports -->
              <div class="text-center py-5" *ngIf="!isLoading && reports.length === 0">
                <i class="fas fa-flag fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No reports submitted</h5>
                <p class="text-muted">You haven't submitted any reports yet.</p>
                <a routerLink="/search" class="btn btn-primary">
                  <i class="fas fa-search me-2"></i>Find Users
                </a>
              </div>

              <!-- Reports Statistics -->
              <div class="row mb-4" *ngIf="!isLoading && reports.length > 0">
                <div class="col-md-3">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-primary mb-1">{{ getTotalReports() }}</h3>
                    <small class="text-muted">Total Reports</small>
                  </div>
                </div>
                <div class="col-md-3">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-warning mb-1">{{ getPendingReports() }}</h3>
                    <small class="text-muted">Pending</small>
                  </div>
                </div>
                <div class="col-md-3">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-success mb-1">{{ getResolvedReports() }}</h3>
                    <small class="text-muted">Resolved</small>
                  </div>
                </div>
                <div class="col-md-3">
                  <div class="stat-card text-center p-3 bg-light rounded">
                    <h3 class="text-secondary mb-1">{{ getDismissedReports() }}</h3>
                    <small class="text-muted">Dismissed</small>
                  </div>
                </div>
              </div>

              <!-- Reports List -->
              <div class="reports-list" *ngIf="!isLoading && reports.length > 0">
                <div class="report-card card mb-3" *ngFor="let report of reports">
                  <div class="card-body">
                    <div class="row">
                      <div class="col-md-8">
                        <!-- Report Header -->
                        <div class="d-flex justify-content-between align-items-start mb-3">
                          <div>
                            <h6 class="mb-1">
                              Report against <strong>{{ report.reportedUserFullName }}</strong>
                            </h6>
                            <small class="text-muted">{{ '@' + report.reportedUserName }}</small>
                          </div>
                          <span class="badge" [ngClass]="getStatusBadgeClass(report.status)">
                            <i [class]="getStatusIcon(report.status)" class="me-1"></i>
                            {{ getStatusText(report.status) }}
                          </span>
                        </div>

                        <!-- Report Details -->
                        <div class="report-details mb-3">
                          <div class="mb-2">
                            <strong>Reason:</strong>
                            <span class="ms-2 badge bg-secondary">{{ report.reason }}</span>
                          </div>
                          
                          <div *ngIf="report.description" class="mb-2">
                            <strong>Description:</strong>
                            <p class="mb-0 mt-1 text-muted">{{ report.description }}</p>
                          </div>
                        </div>

                        <!-- Admin Response -->
                        <div class="admin-response" *ngIf="report.adminNotes && report.status !== ReportStatus.Pending">
                          <div class="alert" [ngClass]="getAdminResponseClass(report.status)">
                            <h6 class="alert-heading">
                              <i class="fas fa-user-shield me-2"></i>Admin Response
                            </h6>
                            <p class="mb-1">{{ report.adminNotes }}</p>
                            <small class="text-muted">
                              Reviewed {{ formatDate(report.reviewedAt!) }}
                              <span *ngIf="report.reviewedByUserName"> by {{ report.reviewedByUserName }}</span>
                            </small>
                          </div>
                        </div>
                      </div>

                      <div class="col-md-4">
                        <!-- Report Timeline -->
                        <div class="report-timeline">
                          <div class="timeline-item">
                            <div class="timeline-marker bg-primary"></div>
                            <div class="timeline-content">
                              <small class="text-muted">Submitted</small>
                              <div class="fw-medium">{{ formatDate(report.timeStamp) }}</div>
                            </div>
                          </div>

                          <div class="timeline-item" *ngIf="report.status === ReportStatus.UnderReview">
                            <div class="timeline-marker bg-warning"></div>
                            <div class="timeline-content">
                              <small class="text-muted">Under Review</small>
                              <div class="fw-medium">In Progress</div>
                            </div>
                          </div>

                          <div class="timeline-item" *ngIf="report.reviewedAt">
                            <div class="timeline-marker" [ngClass]="getTimelineMarkerClass(report.status)"></div>
                            <div class="timeline-content">
                              <small class="text-muted">{{ getStatusText(report.status) }}</small>
                              <div class="fw-medium">{{ formatDate(report.reviewedAt) }}</div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Error Message -->
              <div class="alert alert-danger" *ngIf="errorMessage">
                <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
                <button type="button" class="btn btn-sm btn-outline-danger ms-2" (click)="loadReports()">
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
    .report-card {
      border: none;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .report-card:hover {
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

    .badge {
      font-size: 0.875rem;
      padding: 0.5rem 0.75rem;
    }

    .report-timeline {
      position: relative;
      padding-left: 1.5rem;
    }

    .timeline-item {
      position: relative;
      padding-bottom: 1.5rem;
    }

    .timeline-item:not(:last-child)::before {
      content: '';
      position: absolute;
      left: -1.4rem;
      top: 1rem;
      width: 2px;
      height: calc(100% - 0.5rem);
      background-color: var(--border-color);
    }

    .timeline-marker {
      position: absolute;
      left: -1.75rem;
      top: 0.25rem;
      width: 0.75rem;
      height: 0.75rem;
      border-radius: 50%;
      border: 2px solid white;
      box-shadow: 0 0 0 2px var(--border-color);
    }

    .timeline-content small {
      font-size: 0.75rem;
    }

    .admin-response .alert {
      margin-bottom: 0;
    }

    .alert-heading {
      font-size: 0.9rem;
      margin-bottom: 0.5rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .row.mb-4 {
        gap: 0.5rem;
      }
      
      .col-md-3 {
        flex: 1;
        min-width: 0;
        margin-bottom: 0.5rem;
      }
      
      .report-card .row {
        flex-direction: column;
      }
      
      .report-timeline {
        margin-top: 1rem;
        padding-top: 1rem;
        border-top: 1px solid var(--border-color);
      }
    }
  `]
})
export class ReportsComponent implements OnInit {
  reports: Report[] = [];
  isLoading = false;
  errorMessage = '';
  ReportStatus = ReportStatus;

  constructor(private reportService: ReportService) {}

  ngOnInit() {
    this.loadReports();
  }

  loadReports() {
    this.isLoading = true;
    this.errorMessage = '';

    this.reportService.getMyReports().subscribe({
      next: (reports) => {
        this.reports = reports;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load your reports. Please try again.';
        console.error('Error loading reports:', error);
      }
    });
  }

  getTotalReports(): number {
    return this.reports.length;
  }

  getPendingReports(): number {
    return this.reports.filter(r => r.status === ReportStatus.Pending || r.status === ReportStatus.UnderReview).length;
  }

  getResolvedReports(): number {
    return this.reports.filter(r => r.status === ReportStatus.Resolved).length;
  }

  getDismissedReports(): number {
    return this.reports.filter(r => r.status === ReportStatus.Dismissed).length;
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

  getTimelineMarkerClass(status: ReportStatus): string {
    switch (status) {
      case ReportStatus.Resolved:
        return 'bg-success';
      case ReportStatus.Dismissed:
        return 'bg-secondary';
      default:
        return 'bg-info';
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
