import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule, Validators, FormArray } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';

interface Group {
  groupId: number;
  groupName: string;
  description?: string;
  createdByUserId: string;
  createdByUserName: string;
  createdAt: Date;
  updatedAt: Date;
  isActive: boolean;
  memberCount: number;
  members: any[];
}

interface GroupUser {
  id: string;
  userName: string;
  email: string;
  firstName?: string;
  lastName?: string;
  fullName?: string;
  isActive?: boolean;
}

@Component({
  selector: 'app-group-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-12">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
              <h4 class="mb-0">
                <i class="fas fa-users-cog me-2"></i>Group Management
              </h4>
              <button class="btn btn-warning btn-sm" (click)="showCreateForm = !showCreateForm">
                <i class="fas fa-plus me-2"></i>Create Group
              </button>
            </div>
            <div class="card-body">
              
              <!-- Create Group Form -->
              <div class="card mb-4" *ngIf="showCreateForm">
                <div class="card-header">
                  <h5 class="mb-0">{{ selectedGroup?.groupId ? 'Edit Group' : 'Create New Group' }}</h5>
                </div>
                <div class="card-body">
                  <form [formGroup]="createGroupForm" (ngSubmit)="createGroup()">
                    <div class="row">
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label class="form-label">Group Name *</label>
                          <input
                            type="text"
                            class="form-control"
                            formControlName="groupName"
                            placeholder="Enter group name">
                          <div class="invalid-feedback" 
                               *ngIf="createGroupForm.get('groupName')?.invalid && createGroupForm.get('groupName')?.touched">
                            Group name is required
                          </div>
                        </div>
                      </div>
                      <div class="col-md-6">
                        <div class="mb-3">
                          <label class="form-label">Description</label>
                          <textarea
                            class="form-control"
                            formControlName="description"
                            rows="3"
                            placeholder="Enter group description"></textarea>
                        </div>
                      </div>
                    </div>
                    
                    <div class="mb-3">
                      <label class="form-label">Select Members</label>
                      <div class="row" *ngIf="availableUsers.length > 0">
                        <div class="col-md-6" *ngFor="let user of availableUsers; trackBy: trackByUserId">
                          <div class="form-check">
                            <input
                              class="form-check-input"
                              type="checkbox"
                              [id]="'user-' + user.id"
                              [value]="user.id"
                              [checked]="selectedMemberIds.includes(user.id)"
                              (change)="onMemberSelectionChange($event, user.id)">
                            <label class="form-check-label" [for]="'user-' + user.id">
                              {{ user.fullName || user.userName }} ({{ user.email }})
                            </label>
                          </div>
                        </div>
                      </div>
                      <div *ngIf="availableUsers.length === 0" class="text-muted">
                        <div class="d-flex align-items-center">
                          <div class="spinner-border spinner-border-sm me-2" role="status"></div>
                          Loading users...
                        </div>
                      </div>
                      <div *ngIf="selectedMemberIds.length > 0" class="mt-2">
                        <small class="text-muted">{{ selectedMemberIds.length }} member(s) selected</small>
                      </div>
                    </div>

                    <div class="d-flex gap-2">
                      <button type="submit" class="btn btn-primary" [disabled]="createGroupForm.invalid || isCreating">
                        <span *ngIf="isCreating" class="spinner-border spinner-border-sm me-2"></span>
                        <i *ngIf="!isCreating" class="fas fa-save me-2"></i>
                        {{ isCreating ? (selectedGroup?.groupId ? 'Updating...' : 'Creating...') : (selectedGroup?.groupId ? 'Update Group' : 'Create Group') }}
                      </button>
                      <button type="button" class="btn btn-secondary" (click)="cancelCreate()">
                        Cancel
                      </button>
                    </div>
                  </form>
                </div>
              </div>

              <!-- Search and Filter -->
              <div class="row mb-4">
                <div class="col-md-8">
                  <div class="input-group">
                    <span class="input-group-text">
                      <i class="fas fa-search"></i>
                    </span>
                    <input
                      type="text"
                      class="form-control"
                      placeholder="Search groups by name..."
                      [(ngModel)]="searchTerm"
                      (input)="filterGroups()">
                  </div>
                </div>
                <div class="col-md-4">
                  <button class="btn btn-outline-primary" (click)="loadGroups()">
                    <i class="fas fa-sync-alt me-2"></i>Refresh
                  </button>
                </div>
              </div>

              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-info" role="status">
                  <span class="visually-hidden">Loading groups...</span>
                </div>
                <p class="mt-2 text-muted">Loading groups...</p>
              </div>

              <!-- Groups List -->
              <div class="row" *ngIf="!isLoading && filteredGroups.length > 0">
                <div class="col-lg-6 mb-4" *ngFor="let group of filteredGroups; let i = index">
                  <div class="card h-100">
                    <div class="card-header d-flex justify-content-between align-items-center">
                      <h6 class="mb-0">{{ group.groupName }}</h6>
                      <div class="btn-group btn-group-sm">
                        <button class="btn btn-outline-primary" (click)="viewGroup(group)" title="View Details">
                          <i class="fas fa-eye"></i>
                        </button>
                        <button class="btn btn-outline-warning" (click)="editGroup(group)" title="Edit Group">
                          <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-outline-danger" 
                                (click)="deleteGroup(group, i)"
                                [disabled]="isProcessing[i]" 
                                title="Delete Group">
                          <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm"></span>
                          <i *ngIf="!isProcessing[i]" class="fas fa-trash"></i>
                        </button>
                      </div>
                    </div>
                    <div class="card-body">
                      <p class="text-muted mb-2">{{ group.description || 'No description' }}</p>
                      <div class="d-flex justify-content-between align-items-center">
                        <small class="text-muted">
                          <i class="fas fa-users me-1"></i>{{ group.memberCount }} members
                        </small>
                        <small class="text-muted">
                          Created: {{ formatDate(group.createdAt) }}
                        </small>
                      </div>
                      <div class="mt-2">
                        <small class="text-muted">
                          <i class="fas fa-user me-1"></i>Created by: {{ group.createdByUserName }}
                        </small>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <!-- No Groups -->
              <div class="text-center py-5" *ngIf="!isLoading && filteredGroups.length === 0">
                <i class="fas fa-users-slash fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No groups found</h5>
                <p class="text-muted">No groups match your current search criteria.</p>
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

    <!-- Group Details Modal -->
    <div class="modal fade" id="groupDetailsModal" tabindex="-1" *ngIf="selectedGroup">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="fas fa-users me-2"></i>{{ selectedGroup.groupName }}
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <div class="row">
              <div class="col-md-6">
                <h6>Group Information</h6>
                <table class="table table-borderless">
                  <tr>
                    <th width="40%">Name:</th>
                    <td>{{ selectedGroup.groupName }}</td>
                  </tr>
                  <tr>
                    <th>Description:</th>
                    <td>{{ selectedGroup.description || 'No description' }}</td>
                  </tr>
                  <tr>
                    <th>Created by:</th>
                    <td>{{ selectedGroup.createdByUserName }}</td>
                  </tr>
                  <tr>
                    <th>Created:</th>
                    <td>{{ formatDate(selectedGroup.createdAt) }}</td>
                  </tr>
                  <tr>
                    <th>Members:</th>
                    <td>{{ selectedGroup.memberCount }}</td>
                  </tr>
                </table>
              </div>
              <div class="col-md-6">
                <h6>Members</h6>
                <div class="list-group" style="max-height: 300px; overflow-y: auto;">
                  <div class="list-group-item" *ngFor="let member of selectedGroup.members">
                    <div class="d-flex align-items-center">
                      <div>
                        <div class="fw-medium">{{ member.firstName }} {{ member.lastName }}</div>
                        <small class="text-muted">{{ member.email }}</small>
                      </div>
                    </div>
                  </div>
                  <div *ngIf="selectedGroup.members.length === 0" class="text-muted text-center py-3">
                    No members in this group
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .card {
      transition: transform 0.2s ease;
    }

    .card:hover {
      transform: translateY(-2px);
    }

    .btn-group-sm .btn {
      padding: 0.25rem 0.5rem;
    }

    .form-check {
      margin-bottom: 0.5rem;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .btn-group-sm .btn {
        padding: 0.125rem 0.25rem;
      }
      
      .btn-group-sm .btn i {
        font-size: 0.75rem;
      }
    }
  `]
})
export class GroupManagementComponent implements OnInit {
  groups: Group[] = [];
  filteredGroups: Group[] = [];
  availableUsers: GroupUser[] = [];
  selectedGroup: Group | null = null;
  
  isLoading = false;
  isCreating = false;
  isProcessing: boolean[] = [];
  showCreateForm = false;
  
  successMessage = '';
  errorMessage = '';
  searchTerm = '';
  
  createGroupForm: FormGroup;
  selectedMemberIds: string[] = [];

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService
  ) {
    this.createGroupForm = this.fb.group({
      groupName: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.maxLength(500)]
    });
  }

  ngOnInit() {
    this.loadGroups();
    this.loadUsers();
    console.log('Group management component initialized');
  }

  loadGroups() {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getGroups(1, 100).subscribe({
      next: (groups) => {
        this.groups = groups;
        this.filterGroups();
        this.isLoading = false;
        this.initializeProcessingArray();
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load groups. Please try again.';
        console.error('Error loading groups:', error);
      }
    });
  }

  loadUsers() {
    console.log('Loading users for group creation...');
    this.adminService.getUsers(1, 1000).subscribe({
      next: (users) => {
        console.log('Raw users received:', users.length);
        // Only include active users for group membership
        this.availableUsers = users
          .filter(user => user.isActive === true) // Explicitly check for true
          .map(user => ({
            id: user.id,
            userName: user.userName,
            email: user.email,
            firstName: user.firstName,
            lastName: user.lastName,
            fullName: `${user.firstName || ''} ${user.lastName || ''}`.trim() || user.userName,
            isActive: user.isActive
          }));
        console.log('Active users loaded for group creation:', this.availableUsers.length);
        console.log('Available users:', this.availableUsers.map(u => ({ id: u.id, name: u.fullName, active: u.isActive })));
        console.log('selectedMemberIds after loading users:', this.selectedMemberIds);
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.errorMessage = 'Failed to load users for group creation.';
        this.availableUsers = []; // Ensure empty array on error
      }
    });
  }

  trackByUserId(index: number, user: GroupUser): string {
    return user.id;
  }

  initializeProcessingArray() {
    this.isProcessing = new Array(this.groups.length).fill(false);
  }

  filterGroups() {
    this.filteredGroups = this.groups.filter(group => {
      return !this.searchTerm || 
        group.groupName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        (group.description && group.description.toLowerCase().includes(this.searchTerm.toLowerCase()));
    });
  }

  onMemberSelectionChange(event: any, userId: string) {
    console.log('=== Member Selection Change ===');
    console.log('User ID:', userId);
    console.log('Checked:', event.target.checked);
    console.log('Current selectedMemberIds before change:', this.selectedMemberIds);
    
    if (event.target.checked) {
      if (!this.selectedMemberIds.includes(userId)) {
        this.selectedMemberIds.push(userId);
        console.log('Added user to selection');
      } else {
        console.log('User already selected');
      }
    } else {
      const index = this.selectedMemberIds.indexOf(userId);
      if (index > -1) {
        this.selectedMemberIds.splice(index, 1);
        console.log('Removed user from selection');
      } else {
        console.log('User was not in selection');
      }
    }
    console.log('Selected member IDs after change:', this.selectedMemberIds);
    console.log('Total selected:', this.selectedMemberIds.length);
    console.log('===============================');
  }

  createGroup() {
    if (this.createGroupForm.invalid) return;

    this.isCreating = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formData = {
      groupName: this.createGroupForm.get('groupName')?.value,
      description: this.createGroupForm.get('description')?.value,
      memberIds: this.selectedMemberIds
    };
    
    console.log('Creating group with data:', JSON.stringify(formData, null, 2));
    console.log('Selected member IDs:', this.selectedMemberIds);
    console.log('Member IDs type:', typeof this.selectedMemberIds);
    console.log('Is array?', Array.isArray(this.selectedMemberIds));
    
    // Validate data before sending
    if (!formData.groupName || formData.groupName.trim() === '') {
      this.errorMessage = 'Group name is required';
      this.isCreating = false;
      return;
    }

    // Check if we're editing an existing group
    if (this.selectedGroup && this.selectedGroup.groupId) {
      // Update existing group
      console.log('Updating group with ID:', this.selectedGroup.groupId, 'and data:', formData);
      this.adminService.updateGroup(this.selectedGroup.groupId, formData).subscribe({
        next: (response) => {
          console.log('Group updated successfully:', response);
          this.isCreating = false;
          this.successMessage = 'Group updated successfully!';
          this.cancelCreate();
          this.loadGroups();
        },
        error: (error) => {
          this.isCreating = false;
          console.error('Full error object:', error);
          console.error('Error status:', error.status);
          console.error('Error message:', error.message);
          console.error('Error details:', error.error);
          
          if (error.status === 400) {
            this.errorMessage = 'Invalid data provided. Please check your selections.';
          } else if (error.status === 404) {
            this.errorMessage = 'Group not found. It may have been deleted.';
          } else if (error.status === 500) {
            this.errorMessage = 'Server error occurred. Please try again later.';
          } else {
            this.errorMessage = error.error?.message || 'Failed to update group. Please try again.';
          }
        }
      });
    } else {
      // Create new group
      this.adminService.createGroup(formData).subscribe({
        next: (response) => {
          console.log('Group created successfully:', response);
          this.isCreating = false;
          this.successMessage = 'Group created successfully!';
          this.cancelCreate();
          this.loadGroups();
        },
        error: (error) => {
          this.isCreating = false;
          this.errorMessage = error.error?.message || 'An error occurred while creating the group';
          console.error('Error creating group:', error);
          console.error('Error details:', error.error);
        }
      });
    }
  }

  cancelCreate() {
    this.showCreateForm = false;
    this.createGroupForm.reset();
    this.selectedMemberIds = [];
    this.selectedGroup = null; // Reset selected group for edit mode
    this.errorMessage = '';
    this.successMessage = '';
    
    console.log('Form cancelled, selectedMemberIds cleared:', this.selectedMemberIds);
  }

  viewGroup(group: Group) {
    this.selectedGroup = group;
    const modal = document.getElementById('groupDetailsModal');
    if (modal) {
      const bootstrapModal = new (window as any).bootstrap.Modal(modal);
      bootstrapModal.show();
    }
  }

  editGroup(group: Group) {
    this.selectedGroup = group;
    this.showCreateForm = true;
    this.errorMessage = '';
    this.successMessage = '';
    
    // Pre-populate the form with existing group data
    this.createGroupForm.patchValue({
      groupName: group.groupName,
      description: group.description || ''
    });
    
    // Pre-select existing members
    this.selectedMemberIds = group.members ? group.members.map(member => member.id) : [];
    
    console.log('Editing group:', group.groupName, 'with members:', this.selectedMemberIds);
  }

  deleteGroup(group: Group, index: number) {
    if (!confirm(`Are you sure you want to delete the group "${group.groupName}"? This action cannot be undone.`)) {
      return;
    }

    this.isProcessing[index] = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.adminService.deleteGroup(group.groupId).subscribe({
      next: () => {
        this.isProcessing[index] = false;
        this.successMessage = 'Group deleted successfully!';
        
        // Remove group from local arrays
        const groupIndex = this.groups.findIndex(g => g.groupId === group.groupId);
        if (groupIndex > -1) {
          this.groups.splice(groupIndex, 1);
          this.filterGroups();
        }
      },
      error: (error) => {
        this.isProcessing[index] = false;
        this.errorMessage = error.error?.message || 'Failed to delete group. Please try again.';
        console.error('Error deleting group:', error);
      }
    });
  }

  formatDate(date?: Date): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }
}
