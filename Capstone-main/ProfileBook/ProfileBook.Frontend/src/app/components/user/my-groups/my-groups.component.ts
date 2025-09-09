import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserGroupsService, UserGroup } from '../../../services/user-groups.service';

@Component({
  selector: 'app-my-groups',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-10">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
              <h4 class="mb-0">
                <i class="fas fa-users me-2"></i>My Groups
              </h4>
              <span class="badge bg-warning text-dark">{{ groups.length }} groups</span>
            </div>
            <div class="card-body">
              
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading groups...</span>
                </div>
                <p class="mt-2 text-muted">Loading your groups...</p>
              </div>

              <!-- Groups List -->
              <div class="row" *ngIf="!isLoading && groups.length > 0">
                <div class="col-lg-6 mb-4" *ngFor="let group of groups; let i = index">
                  <div class="card h-100 border-0 shadow-sm">
                    <div class="card-header bg-secondary d-flex justify-content-between align-items-center">
                      <h6 class="mb-0 text-white">{{ group.groupName }}</h6>
                      <button class="btn btn-warning btn-sm" (click)="openGroupChat(group)">
                        <i class="fas fa-comments me-1"></i>Chat
                      </button>
                    </div>
                    <div class="card-body">
                      <p class="text-muted mb-2" *ngIf="group.description">{{ group.description }}</p>
                      <p class="text-muted mb-2" *ngIf="!group.description">No description available</p>
                      
                      <div class="d-flex justify-content-between align-items-center mb-2">
                        <small class="text-muted">
                          <i class="fas fa-users me-1"></i>{{ group.memberCount }} members
                        </small>
                        <small class="text-muted">
                          Created: {{ formatDate(group.createdAt) }}
                        </small>
                      </div>
                      
                      <div class="mb-3">
                        <small class="text-muted">
                          <i class="fas fa-user me-1"></i>Created by: {{ group.createdByUserName }}
                        </small>
                      </div>

                      <!-- Members Preview -->
                      <div class="mb-2" *ngIf="group.members && group.members.length > 0">
                        <small class="text-muted d-block mb-1">Members:</small>
                        <div class="d-flex flex-wrap gap-1">
                          <span 
                            class="badge bg-info" 
                            *ngFor="let member of group.members.slice(0, 3)"
                            [title]="member.firstName + ' ' + member.lastName">
                            {{ member.firstName }} {{ member.lastName }}
                          </span>
                          <span class="badge bg-success text-white" *ngIf="group.members.length > 3">
                            +{{ group.members.length - 3 }} more
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- No Groups -->
              <div class="text-center py-5" *ngIf="!isLoading && groups.length === 0">
                <i class="fas fa-users-slash fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No Groups Found</h5>
                <p class="text-muted">You are not a member of any groups yet. Groups are created by administrators.</p>
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
    .card {
      transition: transform 0.2s ease, box-shadow 0.2s ease;
    }

    .card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0,0,0,0.1) !important;
    }

    .badge {
      font-size: 0.75rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
    }
  `]
})
export class MyGroupsComponent implements OnInit {
  groups: UserGroup[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private userGroupsService: UserGroupsService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadMyGroups();
  }

  loadMyGroups() {
    this.isLoading = true;
    this.errorMessage = '';

    this.userGroupsService.getMyGroups().subscribe({
      next: (groups) => {
        this.groups = groups;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load your groups. Please try again.';
        console.error('Error loading groups:', error);
      }
    });
  }

  openGroupChat(group: UserGroup) {
    this.router.navigate(['/group-chat', group.groupId]);
  }

  formatDate(date?: Date): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }
}
