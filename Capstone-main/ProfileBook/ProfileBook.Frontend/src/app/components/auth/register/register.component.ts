import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { RegisterRequest } from '../../../models/user.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="auth-container">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-md-8 col-lg-6">
            <div class="card shadow-lg">
              <div class="card-body p-5">
                <div class="text-center mb-4">
                  <h1 class="h3 mb-3 fw-bold text-primary">
                    <i class="fas fa-users me-2"></i>ProfileBook
                  </h1>
                  <p class="text-muted">Create your account</p>
                </div>

                <form [formGroup]="registerForm" (ngSubmit)="onSubmit()">
                  <div class="row">
                    <div class="col-md-6 mb-3">
                      <label for="firstName" class="form-label">First Name</label>
                      <input
                        type="text"
                        class="form-control"
                        id="firstName"
                        formControlName="firstName"
                        [class.is-invalid]="isFieldInvalid('firstName')"
                        placeholder="Enter your first name"
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
                        placeholder="Enter your last name"
                      >
                      <div class="invalid-feedback" *ngIf="isFieldInvalid('lastName')">
                        Last name is required
                      </div>
                    </div>
                  </div>

                  <div class="mb-3">
                    <label for="email" class="form-label">Email</label>
                    <input
                      type="email"
                      class="form-control"
                      id="email"
                      formControlName="email"
                      [class.is-invalid]="isFieldInvalid('email')"
                      placeholder="Enter your email"
                    >
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('email')">
                      <span *ngIf="registerForm.get('email')?.errors?.['required']">Email is required</span>
                      <span *ngIf="registerForm.get('email')?.errors?.['email']">Please enter a valid email</span>
                    </div>
                  </div>

                  <div class="mb-3">
                    <label for="userName" class="form-label">Username</label>
                    <input
                      type="text"
                      class="form-control"
                      id="userName"
                      formControlName="userName"
                      [class.is-invalid]="isFieldInvalid('userName')"
                      placeholder="Choose a username"
                    >
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('userName')">
                      Username is required
                    </div>
                  </div>

                  <div class="mb-3">
                    <label for="bio" class="form-label">Bio (Optional)</label>
                    <textarea
                      class="form-control"
                      id="bio"
                      formControlName="bio"
                      rows="3"
                      placeholder="Tell us about yourself..."
                    ></textarea>
                  </div>

                  <div class="row">
                    <div class="col-md-6 mb-3">
                      <label for="password" class="form-label">Password</label>
                      <input
                        type="password"
                        class="form-control"
                        id="password"
                        formControlName="password"
                        [class.is-invalid]="isFieldInvalid('password')"
                        placeholder="Create a password"
                      >
                      <div class="invalid-feedback" *ngIf="isFieldInvalid('password')">
                        Password must be at least 6 characters
                      </div>
                    </div>

                    <div class="col-md-6 mb-3">
                      <label for="confirmPassword" class="form-label">Confirm Password</label>
                      <input
                        type="password"
                        class="form-control"
                        id="confirmPassword"
                        formControlName="confirmPassword"
                        [class.is-invalid]="isFieldInvalid('confirmPassword')"
                        placeholder="Confirm your password"
                      >
                      <div class="invalid-feedback" *ngIf="isFieldInvalid('confirmPassword')">
                        <span *ngIf="registerForm.get('confirmPassword')?.errors?.['required']">Please confirm your password</span>
                        <span *ngIf="registerForm.get('confirmPassword')?.errors?.['passwordMismatch']">Passwords do not match</span>
                      </div>
                    </div>
                  </div>

                  <div class="alert alert-danger" *ngIf="errorMessage">
                    <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
                  </div>

                  <button
                    type="submit"
                    class="btn btn-primary w-100 mb-3"
                    [disabled]="registerForm.invalid || isLoading"
                  >
                    <span *ngIf="isLoading" class="spinner-border spinner-border-sm me-2"></span>
                    <i *ngIf="!isLoading" class="fas fa-user-plus me-2"></i>
                    {{ isLoading ? 'Creating Account...' : 'Create Account' }}
                  </button>

                  <div class="text-center">
                    <p class="mb-0">
                      Already have an account?
                      <a routerLink="/login" class="text-primary fw-medium">Sign in</a>
                    </p>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .auth-container {
      min-height: 100vh;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      display: flex;
      align-items: center;
      padding: 2rem 0;
    }

    .card {
      border: none;
      border-radius: 1rem;
    }

    .form-control {
      padding: 0.75rem 1rem;
      border-radius: 0.5rem;
    }

    .btn-primary {
      padding: 0.75rem;
      border-radius: 0.5rem;
      font-weight: 600;
    }

    .text-primary {
      color: var(--primary-color) !important;
    }

    textarea.form-control {
      resize: vertical;
      min-height: 80px;
    }

    @media (max-width: 768px) {
      .auth-container {
        padding: 1rem;
      }
      
      .card-body {
        padding: 2rem !important;
      }
    }
  `]
})
export class RegisterComponent {
  registerForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', [Validators.required]],
      bio: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else if (confirmPassword?.errors?.['passwordMismatch']) {
      delete confirmPassword.errors['passwordMismatch'];
      if (Object.keys(confirmPassword.errors).length === 0) {
        confirmPassword.setErrors(null);
      }
    }
    return null;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.registerForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const registerRequest: RegisterRequest = this.registerForm.value;

      this.authService.register(registerRequest).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.router.navigate(['/feed']);
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Registration error:', error);
          this.errorMessage = error.message || 'Registration failed. Please try again.';
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched() {
    Object.keys(this.registerForm.controls).forEach(key => {
      const control = this.registerForm.get(key);
      control?.markAsTouched();
    });
  }
}
