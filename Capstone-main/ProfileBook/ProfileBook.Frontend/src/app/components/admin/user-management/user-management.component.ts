import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { AdminService } from '../../../services/admin.service';
import { User } from '../../../models/user.model';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-12">
          <div class="card shadow-sm">
            <div class="card-header bg-success text-white">
              <h4 class="mb-0">
                <i class="fas fa-users me-2"></i>User Management
              </h4>
            </div>
            <div class="card-body">
              <!-- Search and Filter -->
              <div class="row mb-4">
                <div class="col-md-6">
                  <div class="input-group">
                    <span class="input-group-text">
                      <i class="fas fa-search"></i>
                    </span>
                    <input
                      type="text"
                      class="form-control"
                      placeholder="Search users by name or email..."
                      [(ngModel)]="searchTerm"
                      (input)="onSearchChange()"
                    >
                  </div>
                </div>
                <div class="col-md-3">
                  <select class="form-select" [(ngModel)]="selectedRole" (change)="filterUsers()">
                    <option value="">All Roles</option>
                    <option value="User">Users</option>
                    <option value="Admin">Admins</option>
                  </select>
                </div>
                <div class="col-md-3">
                  <div class="d-flex gap-2">
                    <button class="btn btn-success" (click)="showCreateUserModal()">
                      <i class="fas fa-plus me-2"></i>Create User
                    </button>
                    <button class="btn btn-primary" (click)="loadUsers()">
                      <i class="fas fa-sync-alt me-2"></i>Refresh
                    </button>
                  </div>
                </div>
              </div>

              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-success" role="status">
                  <span class="visually-hidden">Loading users...</span>
                </div>
                <p class="mt-2 text-muted">Loading users...</p>
              </div>

              <!-- Users Table -->
              <div class="table-responsive" *ngIf="!isLoading && filteredUsers.length > 0">
                <table class="table table-hover">
                  <thead class="table-light">
                    <tr>
                      <th>User</th>
                      <th>Email</th>
                      <th>Role</th>
                      <th>Joined</th>
                      <th>Status</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr *ngFor="let user of paginatedUsers; let i = index">
                      <td>
                        <div class="d-flex align-items-center">
                          <img [src]="getUserAvatar(user)" 
                               class="rounded-circle me-2" 
                               width="32" 
                               height="32" 
                               alt="User avatar"
                               (error)="onImageError($event, user)"
                               [style.background-color]="'#f8f9fa'">
                          <div>
                            <div class="fw-medium">{{ user.fullName || 'No Name' }}</div>
                            <small class="text-muted">{{ '@' + user.userName }}</small>
                          </div>
                        </div>
                      </td>
                      <td>{{ user.email }}</td>
                      <td>
                        <span class="badge" [ngClass]="getRoleBadgeClass(user.roles)">
                          {{ getUserRole(user.roles) }}
                        </span>
                      </td>
                      <td>{{ formatDate(user.createdAt) }}</td>
                      <td>
                        <span class="badge" [ngClass]="getStatusBadgeClass(user.lockoutEnd)">
                          {{ getUserStatus(user.lockoutEnd) }}
                        </span>
                      </td>
                      <td>
                        <div class="btn-group btn-group-sm">
                          <button class="btn btn-outline-primary" 
                                  (click)="viewUser(user)"
                                  title="View Details">
                            <i class="fas fa-eye"></i>
                          </button>
                          <button class="btn btn-outline-info" 
                                  (click)="editUser(user)"
                                  title="Edit User">
                            <i class="fas fa-edit"></i>
                          </button>
                          <button class="btn btn-outline-warning" 
                                  (click)="toggleUserStatus(user, i)"
                                  [disabled]="isProcessing[i]"
                                  [title]="getUserStatus(user.lockoutEnd) === 'Active' ? 'Suspend User' : 'Activate User'">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm"></span>
                            <i *ngIf="!isProcessing[i]" [class]="getUserStatus(user.lockoutEnd) === 'Active' ? 'fas fa-ban' : 'fas fa-check'"></i>
                          </button>
                          <button class="btn btn-outline-danger" 
                                  (click)="deleteUser(user, i)"
                                  [disabled]="isProcessing[i] || user.roles?.includes('Admin')"
                                  title="Delete User">
                            <span *ngIf="isProcessing[i]" class="spinner-border spinner-border-sm"></span>
                            <i *ngIf="!isProcessing[i]" class="fas fa-trash"></i>
                          </button>
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>

              <!-- No Users -->
              <div class="text-center py-5" *ngIf="!isLoading && filteredUsers.length === 0">
                <i class="fas fa-users fa-4x text-muted mb-3"></i>
                <h5 class="text-muted">No users found</h5>
                <p class="text-muted">No users match your current search criteria.</p>
              </div>

              <!-- Pagination -->
              <nav *ngIf="!isLoading && filteredUsers.length > pageSize">
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

    <!-- User Details Modal -->
    <div class="modal fade" id="userDetailsModal" tabindex="-1" *ngIf="selectedUser">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="fas fa-user me-2"></i>User Details
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <div class="row">
              <div class="col-md-4 text-center mb-3">
                <img [src]="getUserAvatar(selectedUser)" class="rounded-circle mb-2" 
                     width="100" height="100" alt="User avatar">
                <h5>{{ selectedUser.fullName || 'No Name' }}</h5>
                <p class="text-muted">{{ '@' + selectedUser.userName }}</p>
              </div>
              <div class="col-md-8">
                <table class="table table-borderless">
                  <tr>
                    <th width="30%">Email:</th>
                    <td>{{ selectedUser.email }}</td>
                  </tr>
                  <tr>
                    <th>Role:</th>
                    <td>
                      <span class="badge" [ngClass]="getRoleBadgeClass(selectedUser.roles)">
                        {{ getUserRole(selectedUser.roles) }}
                      </span>
                    </td>
                  </tr>
                  <tr>
                    <th>Status:</th>
                    <td>
                      <span class="badge" [ngClass]="getStatusBadgeClass(selectedUser.lockoutEnd)">
                        {{ getUserStatus(selectedUser.lockoutEnd) }}
                      </span>
                    </td>
                  </tr>
                  <tr>
                    <th>Bio:</th>
                    <td>{{ selectedUser.bio || 'No bio provided' }}</td>
                  </tr>
                  <tr>
                    <th>Joined:</th>
                    <td>{{ formatDate(selectedUser.createdAt) }}</td>
                  </tr>
                  <tr *ngIf="selectedUser.lockoutEnd">
                    <th>Suspended Until:</th>
                    <td>{{ formatDate(selectedUser.lockoutEnd) }}</td>
                  </tr>
                </table>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Create/Edit User Modal -->
    <div class="modal fade" id="createUserModal" tabindex="-1">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="fas" [class.fa-user-plus]="!editingUser" [class.fa-user-edit]="editingUser" class="me-2"></i>
              {{ editingUser ? 'Edit User' : 'Create New User' }}
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <form [formGroup]="createUserForm" (ngSubmit)="editingUser ? updateUser() : createUser()">
            <div class="modal-body">
              <div class="row">
                <div class="col-md-6">
                  <div class="mb-3">
                    <label class="form-label">First Name *</label>
                    <input type="text" class="form-control" formControlName="firstName" 
                           [class.is-invalid]="isFieldInvalid('firstName')">
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('firstName')">
                      First name is required
                    </div>
                  </div>
                </div>
                <div class="col-md-6">
                  <div class="mb-3">
                    <label class="form-label">Last Name *</label>
                    <input type="text" class="form-control" formControlName="lastName"
                           [class.is-invalid]="isFieldInvalid('lastName')">
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('lastName')">
                      Last name is required
                    </div>
                  </div>
                </div>
              </div>
              
              <div class="mb-3">
                <label class="form-label">Username *</label>
                <input type="text" class="form-control" formControlName="userName"
                       [class.is-invalid]="isFieldInvalid('userName')">
                <div class="invalid-feedback" *ngIf="isFieldInvalid('userName')">
                  Username is required and must be at least 3 characters
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">Email *</label>
                <input type="email" class="form-control" formControlName="email"
                       [class.is-invalid]="isFieldInvalid('email')">
                <div class="invalid-feedback" *ngIf="isFieldInvalid('email')">
                  Valid email is required
                </div>
              </div>

              <div class="mb-3" *ngIf="!editingUser">
                <label class="form-label">Password *</label>
                <input type="password" class="form-control" formControlName="password"
                       [class.is-invalid]="isFieldInvalid('password')">
                <div class="invalid-feedback" *ngIf="isFieldInvalid('password')">
                  Password must be at least 6 characters
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">Role *</label>
                <select class="form-select" formControlName="role"
                        [class.is-invalid]="isFieldInvalid('role')">
                  <option value="">Select Role</option>
                  <option value="User">User</option>
                  <option value="Admin">Admin</option>
                </select>
                <div class="invalid-feedback" *ngIf="isFieldInvalid('role')">
                  Role is required
                </div>
              </div>

              <div class="mb-3">
                <label class="form-label">Bio (Optional)</label>
                <textarea class="form-control" formControlName="bio" rows="3"
                          placeholder="User bio..."></textarea>
              </div>
            </div>
            <div class="modal-footer">
              <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
              <button type="submit" class="btn btn-success" [disabled]="createUserForm.invalid || isCreatingUser">
                <span *ngIf="isCreatingUser" class="spinner-border spinner-border-sm me-2"></span>
                <i *ngIf="!isCreatingUser" class="fas fa-save me-2"></i>
                {{ isCreatingUser ? (editingUser ? 'Updating...' : 'Creating...') : (editingUser ? 'Update User' : 'Create User') }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .table th {
      border-top: none;
      font-weight: 600;
    }

    .btn-group-sm .btn {
      padding: 0.25rem 0.5rem;
    }

    .badge {
      font-size: 0.75rem;
    }

    .modal-body table th {
      color: var(--text-muted);
      font-weight: 500;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .table-responsive {
        font-size: 0.875rem;
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
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  filteredUsers: User[] = [];
  paginatedUsers: User[] = [];
  isLoading = false;
  isProcessing: boolean[] = [];
  successMessage = '';
  errorMessage = '';
  selectedUser: User | null = null;

  // Create/Edit User Form
  createUserForm: FormGroup;
  isCreatingUser = false;
  editingUser: User | null = null;

  // Search and Filter
  searchTerm = '';
  selectedRole = '';
  private searchTimeout: any;

  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService
  ) {
    this.createUserForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      userName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['', [Validators.required]],
      bio: ['']
    });
  }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getUsers(1, 1000).subscribe({ // Load all users for client-side filtering
      next: (users) => {
        // Fix: Add fullName property for display
        this.users = users.map(user => ({
          ...user,
          fullName: `${user.firstName || ''} ${user.lastName || ''}`.trim() || user.userName
        }));
        this.filterUsers();
        this.isLoading = false;
        this.initializeProcessingArray();
      },
      error: (error) => {
        this.isLoading = false;
        if (error.status === 401) {
          this.errorMessage = 'Authentication failed. Please log in as an admin.';
        } else if (error.status === 403) {
          this.errorMessage = 'Access denied. Admin privileges required.';
        } else {
          this.errorMessage = 'Failed to load users. Please try again.';
        }
        console.error('Error loading users:', error);
      }
    });
  }

  initializeProcessingArray() {
    this.isProcessing = new Array(this.users.length).fill(false);
  }

  onSearchChange() {
    clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(() => {
      this.filterUsers();
    }, 300);
  }

  filterUsers() {
    this.filteredUsers = this.users.filter(user => {
      const matchesSearch = !this.searchTerm || 
        user.fullName?.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        user.userName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchTerm.toLowerCase());

      const matchesRole = !this.selectedRole || 
        user.roles?.includes(this.selectedRole);

      // Always exclude inactive/deleted users
      const isActive = user.isActive !== false;

      return matchesSearch && matchesRole && isActive;
    });

    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredUsers.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedUsers = this.filteredUsers.slice(startIndex, endIndex);
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

  viewUser(user: User) {
    this.selectedUser = user;
    // In a real app, you'd use a modal service or library
    const modal = document.getElementById('userDetailsModal');
    if (modal) {
      const bootstrapModal = new (window as any).bootstrap.Modal(modal);
      bootstrapModal.show();
    }
  }

  toggleUserStatus(user: User, index: number) {
    this.isProcessing[index] = true;
    this.errorMessage = '';
    this.successMessage = '';

    const isCurrentlyActive = this.getUserStatus(user.lockoutEnd) === 'Active';
    const action = isCurrentlyActive ? 'suspend' : 'activate';

    this.adminService.toggleUserStatus(user.id, !isCurrentlyActive).subscribe({
      next: () => {
        this.isProcessing[index] = false;
        this.successMessage = `User ${action}d successfully!`;
        
        // Update the user's status locally
        if (isCurrentlyActive) {
          user.lockoutEnd = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000); // 30 days from now
        } else {
          user.lockoutEnd = null;
        }
      },
      error: (error) => {
        this.isProcessing[index] = false;
        if (error.status === 401) {
          this.errorMessage = 'Authentication failed. Please log in as an admin.';
        } else if (error.status === 403) {
          this.errorMessage = 'Access denied. Admin privileges required.';
        } else {
          this.errorMessage = 'Failed to update user status. Please try again.';
        }
        console.error('Error updating user status:', error);
      }
    });
  }

  deleteUser(user: User, index: number) {
    if (!confirm(`Are you sure you want to delete user "${user.fullName || user.userName}"? This action cannot be undone.`)) {
      return;
    }

    this.isProcessing[index] = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.adminService.deleteUser(user.id).subscribe({
      next: () => {
        this.isProcessing[index] = false;
        this.successMessage = 'User deleted successfully!';
        
        // Fix: Mark user as inactive instead of removing
        const userIndex = this.users.findIndex(u => u.id === user.id);
        if (userIndex > -1) {
          this.users[userIndex].isActive = false;
          this.filterUsers();
        }
      },
      error: (error) => {
        this.isProcessing[index] = false;
        this.errorMessage = error.error?.message || 'Failed to delete user. Please try again.';
        console.error('Error deleting user:', error);
      }
    });
  }

  getUserAvatar(user: User): string {
    console.log('Getting avatar for user:', user); // Debug log
    
    if (user.profileImagePath) {
      // Handle both relative and absolute paths
      if (user.profileImagePath.startsWith('http')) {
        return user.profileImagePath;
      }
      // For relative paths, construct the full URL to the API
      return `https://localhost:5120${user.profileImagePath}`;
    }
    
    // Generate a more professional default avatar with user initials
    const initials = this.getUserInitials(user);
    console.log('Generated initials:', initials); // Debug log
    
    const colors = [
      { bg: '6366f1', text: 'ffffff' }, // indigo
      { bg: '8b5cf6', text: 'ffffff' }, // violet  
      { bg: '06b6d4', text: 'ffffff' }, // cyan
      { bg: '10b981', text: 'ffffff' }, // emerald
      { bg: 'f59e0b', text: 'ffffff' }, // amber
      { bg: 'ef4444', text: 'ffffff' }, // red
      { bg: 'ec4899', text: 'ffffff' }, // pink
      { bg: '84cc16', text: 'ffffff' }  // lime
    ];
    
    // Use user ID to consistently assign colors
    const colorIndex = user.id ? user.id.charCodeAt(0) % colors.length : 0;
    const color = colors[colorIndex];
    
    const avatarUrl = `https://ui-avatars.com/api/?name=${encodeURIComponent(initials)}&background=${color.bg}&color=${color.text}&size=128&font-size=0.6&rounded=true&bold=true`;
    console.log('Generated avatar URL:', avatarUrl); // Debug log
    
    return avatarUrl;
  }

  private getUserInitials(user: User): string {
    if (user.firstName && user.lastName) {
      return `${user.firstName.charAt(0)}${user.lastName.charAt(0)}`.toUpperCase();
    }
    if (user.fullName) {
      const names = user.fullName.split(' ');
      if (names.length >= 2) {
        return `${names[0].charAt(0)}${names[names.length - 1].charAt(0)}`.toUpperCase();
      }
      return names[0].charAt(0).toUpperCase();
    }
    return user.userName.charAt(0).toUpperCase();
  }

  getUserRole(roles?: string[]): string {
    if (roles?.includes('Admin')) return 'Admin';
    return 'User';
  }

  getRoleBadgeClass(roles?: string[]): string {
    if (roles?.includes('Admin')) return 'bg-danger';
    return 'bg-primary';
  }

  getUserStatus(lockoutEnd?: Date | null): string {
    if (!lockoutEnd) return 'Active';
    const now = new Date();
    const lockout = new Date(lockoutEnd);
    return lockout > now ? 'Suspended' : 'Active';
  }

  getStatusBadgeClass(lockoutEnd?: Date | null): string {
    const status = this.getUserStatus(lockoutEnd);
    return status === 'Active' ? 'bg-success' : 'bg-warning';
  }

  formatDate(date?: Date): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString();
  }

  showCreateUserModal() {
    this.editingUser = null;
    this.createUserForm.reset();
    // Make password required for new users
    this.createUserForm.get('password')?.setValidators([Validators.required, Validators.minLength(6)]);
    this.createUserForm.get('password')?.updateValueAndValidity();
    
    const modal = document.getElementById('createUserModal');
    if (modal) {
      const bootstrapModal = new (window as any).bootstrap.Modal(modal);
      bootstrapModal.show();
    }
  }

  editUser(user: User) {
    this.editingUser = user;
    this.createUserForm.patchValue({
      firstName: user.firstName,
      lastName: user.lastName,
      userName: user.userName,
      email: user.email,
      role: this.getUserRole(user.roles),
      bio: user.bio || ''
    });
    
    // Remove password validation for editing
    this.createUserForm.get('password')?.clearValidators();
    this.createUserForm.get('password')?.updateValueAndValidity();
    
    const modal = document.getElementById('createUserModal');
    if (modal) {
      const bootstrapModal = new (window as any).bootstrap.Modal(modal);
      bootstrapModal.show();
    }
  }

  updateUser() {
    if (this.createUserForm.valid && this.editingUser) {
      this.isCreatingUser = true;
      this.errorMessage = '';
      this.successMessage = '';

      const userData = {
        firstName: this.createUserForm.get('firstName')?.value,
        lastName: this.createUserForm.get('lastName')?.value,
        userName: this.createUserForm.get('userName')?.value,
        email: this.createUserForm.get('email')?.value,
        roles: [this.createUserForm.get('role')?.value],
        isActive: this.editingUser.isActive ?? true
      };

      this.adminService.updateUser(this.editingUser.id, userData).subscribe({
        next: (updatedUser) => {
          this.isCreatingUser = false;
          this.successMessage = 'User updated successfully!';
          
          // Update user in local array
          const userIndex = this.users.findIndex(u => u.id === this.editingUser!.id);
          if (userIndex > -1) {
            this.users[userIndex] = {
              ...updatedUser,
              fullName: `${updatedUser.firstName || ''} ${updatedUser.lastName || ''}`.trim() || updatedUser.userName
            };
            this.filterUsers();
          }
          
          // Close modal
          const modal = document.getElementById('createUserModal');
          if (modal) {
            const bootstrapModal = (window as any).bootstrap.Modal.getInstance(modal);
            if (bootstrapModal) {
              bootstrapModal.hide();
            }
          }
        },
        error: (error) => {
          this.isCreatingUser = false;
          this.errorMessage = error.error?.message || 'Failed to update user. Please try again.';
          console.error('Error updating user:', error);
        }
      });
    } else {
      this.markFormGroupTouched(this.createUserForm);
    }
  }

  createUser() {
    if (this.createUserForm.valid) {
      this.isCreatingUser = true;
      this.errorMessage = '';
      this.successMessage = '';

      const userData = {
        firstName: this.createUserForm.get('firstName')?.value,
        lastName: this.createUserForm.get('lastName')?.value,
        userName: this.createUserForm.get('userName')?.value,
        email: this.createUserForm.get('email')?.value,
        password: this.createUserForm.get('password')?.value,
        roles: [this.createUserForm.get('role')?.value], // Fix: Send as array
        bio: this.createUserForm.get('bio')?.value || undefined
      };

      this.adminService.createUser(userData).subscribe({
        next: (newUser) => {
          this.isCreatingUser = false;
          this.successMessage = 'User created successfully!';
          // Add fullName property to new user
          const userWithFullName = {
            ...newUser,
            fullName: `${newUser.firstName || ''} ${newUser.lastName || ''}`.trim() || newUser.userName
          };
          this.users.unshift(userWithFullName);
          this.filterUsers();
          
          // Close modal
          const modal = document.getElementById('createUserModal');
          if (modal) {
            const bootstrapModal = (window as any).bootstrap.Modal.getInstance(modal);
            if (bootstrapModal) {
              bootstrapModal.hide();
            }
          }
        },
        error: (error) => {
          this.isCreatingUser = false;
          this.errorMessage = error.error?.message || 'Failed to create user. Please try again.';
          console.error('Error creating user:', error);
        }
      });
    } else {
      this.markFormGroupTouched(this.createUserForm);
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.createUserForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onImageError(event: any, user: User) {
    console.log('Image error for user:', user, 'Event:', event);
    // Fallback to a simple colored div with initials
    const img = event.target;
    const initials = this.getUserInitials(user);
    const colors = ['#6366f1', '#8b5cf6', '#06b6d4', '#10b981', '#f59e0b', '#ef4444', '#ec4899', '#84cc16'];
    const colorIndex = user.id ? user.id.charCodeAt(0) % colors.length : 0;
    
    // Create a canvas-based fallback
    const canvas = document.createElement('canvas');
    canvas.width = 32;
    canvas.height = 32;
    const ctx = canvas.getContext('2d');
    
    if (ctx) {
      // Draw background circle
      ctx.fillStyle = colors[colorIndex];
      ctx.beginPath();
      ctx.arc(16, 16, 16, 0, 2 * Math.PI);
      ctx.fill();
      
      // Draw initials
      ctx.fillStyle = '#ffffff';
      ctx.font = 'bold 12px Arial';
      ctx.textAlign = 'center';
      ctx.textBaseline = 'middle';
      ctx.fillText(initials, 16, 16);
      
      img.src = canvas.toDataURL();
    }
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }
}
