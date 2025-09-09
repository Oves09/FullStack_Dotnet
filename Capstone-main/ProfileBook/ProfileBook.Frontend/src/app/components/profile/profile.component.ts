import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { User, UpdateProfileRequest } from '../../models/user.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="container mt-4">
      <div class="row justify-content-center">
        <div class="col-lg-8">
          <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
              <h4 class="mb-0">
                <i class="fas fa-user me-2"></i>My Profile
              </h4>
            </div>
            <div class="card-body">
              <!-- Loading State -->
              <div class="text-center py-5" *ngIf="isLoading">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading profile...</span>
                </div>
                <p class="mt-2 text-muted">Loading your profile...</p>
              </div>

              <!-- Profile Form -->
              <form [formGroup]="profileForm" (ngSubmit)="onUpdateProfile()" *ngIf="!isLoading">
                <!-- Profile Picture Section -->
                <div class="text-center mb-4">
                  <div class="position-relative d-inline-block">
                    <img [src]="getProfileImage()" class="rounded-circle profile-image" 
                         width="120" height="120" alt="Profile Picture">
                    <button type="button" class="btn btn-primary btn-sm position-absolute bottom-0 end-0 rounded-circle"
                            (click)="fileInput.click()" style="width: 36px; height: 36px;">
                      <i class="fas fa-camera"></i>
                    </button>
                  </div>
                  <input type="file" #fileInput (change)="onImageSelected($event)" 
                         accept="image/*" class="d-none">
                  <p class="text-muted mt-2 mb-0">Click the camera icon to change your profile picture</p>
                </div>

                <!-- Basic Information -->
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="firstName" class="form-label">First Name</label>
                    <input
                      type="text"
                      class="form-control"
                      id="firstName"
                      formControlName="firstName"
                      [class.is-invalid]="isFieldInvalid('firstName')"
                    >
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('firstName')">
                      First name is required
                    </div>
                  </div>

                  <div class="col-md-6 mb-3">
                    <label for="lastName" class="form-label">Last Name</label>
                    <input
                      type="text"
                      class="form-control"
                      id="lastName"
                      formControlName="lastName"
                      [class.is-invalid]="isFieldInvalid('lastName')"
                    >
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('lastName')">
                      Last name is required
                    </div>
                  </div>
                </div>

                <!-- Read-only fields -->
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="userName" class="form-label">Username</label>
                    <input
                      type="text"
                      class="form-control"
                      id="userName"
                      [value]="currentUser?.userName"
                      readonly
                    >
                  </div>

                  <div class="col-md-6 mb-3">
                    <label for="email" class="form-label">Email</label>
                    <input
                      type="email"
                      class="form-control"
                      id="email"
                      [value]="currentUser?.email"
                      readonly
                    >
                  </div>
                </div>

                <!-- Bio -->
                <div class="mb-3">
                  <label for="bio" class="form-label">Bio</label>
                  <textarea
                    class="form-control"
                    id="bio"
                    formControlName="bio"
                    rows="4"
                    placeholder="Tell us about yourself..."
                  ></textarea>
                </div>

                <!-- Account Information -->
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label class="form-label">Member Since</label>
                    <input
                      type="text"
                      class="form-control"
                      [value]="formatDate(currentUser?.createdAt)"
                      readonly
                    >
                  </div>

                  <div class="col-md-6 mb-3">
                    <label class="form-label">Account Status</label>
                    <input
                      type="text"
                      class="form-control"
                      [value]="currentUser?.isActive ? 'Active' : 'Inactive'"
                      readonly
                    >
                  </div>
                </div>

                <!-- Roles -->
                <div class="mb-4">
                  <label class="form-label">Roles</label>
                  <div class="d-flex flex-wrap gap-2">
                    <span *ngFor="let role of currentUser?.roles" 
                          class="badge bg-primary rounded-pill">
                      {{ role }}
                    </span>
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

                <!-- Action Buttons -->
                <div class="d-flex justify-content-end gap-2">
                  <button type="button" class="btn btn-outline-secondary" (click)="resetForm()">
                    <i class="fas fa-undo me-1"></i>Reset
                  </button>
                  <button type="submit" class="btn btn-primary" [disabled]="profileForm.invalid || isUpdating">
                    <span *ngIf="isUpdating" class="spinner-border spinner-border-sm me-2"></span>
                    <i *ngIf="!isUpdating" class="fas fa-save me-1"></i>
                    {{ isUpdating ? 'Updating...' : 'Update Profile' }}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .profile-image {
      object-fit: cover;
      border: 4px solid var(--border-color);
      transition: transform 0.2s ease;
    }

    .profile-image:hover {
      transform: scale(1.05);
    }

    .form-control[readonly] {
      background-color: var(--light-bg);
      border-color: var(--border-color);
    }

    .badge {
      font-size: 0.875rem;
      padding: 0.5rem 0.75rem;
    }

    .card-header {
      border-radius: 0.75rem 0.75rem 0 0 !important;
    }

    @media (max-width: 768px) {
      .container {
        padding: 0 1rem;
      }
      
      .profile-image {
        width: 100px !important;
        height: 100px !important;
      }
      
      .d-flex.justify-content-end {
        flex-direction: column;
      }
      
      .d-flex.justify-content-end .btn {
        margin-bottom: 0.5rem;
      }
    }
  `]
})
export class ProfileComponent implements OnInit {
  profileForm: FormGroup;
  currentUser: User | null = null;
  isLoading = false;
  isUpdating = false;
  selectedFile: File | null = null;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService
  ) {
    this.profileForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      bio: ['']
    });
  }

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading = true;
    this.userService.getProfile().subscribe({
      next: (user) => {
        this.currentUser = user;
        this.profileForm.patchValue({
          firstName: user.firstName || '',
          lastName: user.lastName || '',
          bio: user.bio || ''
        });
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load profile. Please try again.';
        console.error('Error loading profile:', error);
      }
    });
  }

  onUpdateProfile() {
    if (this.profileForm.valid) {
      this.isUpdating = true;
      this.errorMessage = '';
      this.successMessage = '';

      const updateRequest: UpdateProfileRequest = {
        firstName: this.profileForm.get('firstName')?.value,
        lastName: this.profileForm.get('lastName')?.value,
        bio: this.profileForm.get('bio')?.value,
        profileImage: this.selectedFile || undefined
      };

      this.userService.updateProfile(updateRequest).subscribe({
        next: (user) => {
          this.isUpdating = false;
          this.currentUser = user;
          this.selectedFile = null;
          this.successMessage = 'Profile updated successfully!';
          
          // Update the current user in auth service
          this.authService.currentUser$.subscribe(currentUser => {
            if (currentUser) {
              const updatedUser = { ...currentUser, ...user };
              localStorage.setItem('user', JSON.stringify(updatedUser));
            }
          });
        },
        error: (error) => {
          this.isUpdating = false;
          this.errorMessage = error.error?.message || 'Failed to update profile. Please try again.';
          console.error('Error updating profile:', error);
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      // Validate file size (max 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.errorMessage = 'Image size must be less than 5MB';
        return;
      }

      // Validate file type
      if (!file.type.startsWith('image/')) {
        this.errorMessage = 'Please select a valid image file';
        return;
      }

      this.selectedFile = file;
      this.errorMessage = '';
    }
  }

  getProfileImage(): string {
    if (this.currentUser?.profileImagePath) {
      return this.getFullImageUrl(this.currentUser.profileImagePath);
    }
    return 'https://via.placeholder.com/120x120/3b82f6/ffffff?text=' + 
           (this.currentUser?.firstName?.charAt(0) || this.currentUser?.userName?.charAt(0) || 'U');
  }

  getFullImageUrl(imagePath: string): string {
    if (!imagePath) return '';
    if (imagePath.startsWith('http')) return imagePath;
    return `http://localhost:5120${imagePath}`;
  }

  formatDate(date: Date | undefined): string {
    if (!date) return 'N/A';
    return new Date(date).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  resetForm() {
    if (this.currentUser) {
      this.profileForm.patchValue({
        firstName: this.currentUser?.firstName || '',
        lastName: this.currentUser?.lastName || '',
        bio: this.currentUser.bio || ''
      });
      this.selectedFile = null;
      this.errorMessage = '';
      this.successMessage = '';
    }
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.profileForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  private markFormGroupTouched() {
    Object.keys(this.profileForm.controls).forEach(key => {
      const control = this.profileForm.get(key);
      control?.markAsTouched();
    });
  }
}
