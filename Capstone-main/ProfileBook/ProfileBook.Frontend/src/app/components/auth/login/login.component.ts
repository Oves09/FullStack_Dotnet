import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { LoginRequest } from '../../../models/user.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  template: `
    <div class="auth-container">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-md-6 col-lg-4">
            <div class="card shadow-lg">
              <div class="card-body p-5">
                <div class="text-center mb-4">
                  <h1 class="h3 mb-3 fw-bold text-primary">
                    <i class="fas fa-users me-2"></i>ProfileBook
                  </h1>
                  <p class="text-muted">Sign in to your account</p>
                </div>

                <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
                  <div class="mb-3">
                    <label for="userName" class="form-label">Username</label>
                    <input
                      type="text"
                      class="form-control"
                      id="userName"
                      formControlName="userName"
                      [class.is-invalid]="isFieldInvalid('userName')"
                      placeholder="Enter your username"
                    >
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('userName')">
                      Username is required
                    </div>
                  </div>

                  <div class="mb-3">
                    <label for="password" class="form-label">Password</label>
                    <input
                      type="password"
                      class="form-control"
                      id="password"
                      formControlName="password"
                      [class.is-invalid]="isFieldInvalid('password')"
                      placeholder="Enter your password"
                    >
                    <div class="invalid-feedback" *ngIf="isFieldInvalid('password')">
                      Password is required
                    </div>
                  </div>

                  <div class="alert alert-danger" *ngIf="errorMessage">
                    <i class="fas fa-exclamation-triangle me-2"></i>{{ errorMessage }}
                  </div>

                  <button
                    type="submit"
                    class="btn btn-primary w-100 mb-3"
                    [disabled]="loginForm.invalid || isLoading"
                  >
                    <span *ngIf="isLoading" class="spinner-border spinner-border-sm me-2"></span>
                    <i *ngIf="!isLoading" class="fas fa-sign-in-alt me-2"></i>
                    {{ isLoading ? 'Signing in...' : 'Sign In' }}
                  </button>

                  <div class="text-center">
                    <p class="mb-0">
                      Don't have an account?
                      <a routerLink="/register" class="text-primary fw-medium">Sign up</a>
                    </p>
                  </div>
                </form>

                <hr class="my-4">
                
                <div class="text-center">
                  <small class="text-muted">
                    <strong>Demo Credentials:</strong><br>
                    Admin: admin / Admin123!<br>
                    Or create a new account
                  </small>
                </div>
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
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.loginForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const loginRequest: LoginRequest = this.loginForm.value;

      this.authService.login(loginRequest).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.router.navigate(['/feed']);
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Login error:', error);
          this.errorMessage = error.message || 'Login failed. Please try again.';
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private markFormGroupTouched() {
    Object.keys(this.loginForm.controls).forEach(key => {
      const control = this.loginForm.get(key);
      control?.markAsTouched();
    });
  }
}
