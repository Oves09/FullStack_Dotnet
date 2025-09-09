import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { RegisterComponent } from './register.component';
import { AuthService } from '../../../services/auth.service';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['register']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [RegisterComponent, ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    mockAuthService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize register form with validators', () => {
    expect(component.registerForm.get('userName')?.hasError('required')).toBeTruthy();
    expect(component.registerForm.get('email')?.hasError('required')).toBeTruthy();
    expect(component.registerForm.get('password')?.hasError('required')).toBeTruthy();
    expect(component.registerForm.get('confirmPassword')?.hasError('required')).toBeTruthy();
  });

  it('should validate email format', () => {
    const emailControl = component.registerForm.get('email');
    
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTruthy();
    
    emailControl?.setValue('valid@email.com');
    expect(emailControl?.hasError('email')).toBeFalsy();
  });

  it('should validate password confirmation', () => {
    component.registerForm.patchValue({
      password: 'password123',
      confirmPassword: 'different123'
    });
    
    expect(component.registerForm.hasError('passwordMismatch')).toBeTruthy();
    
    component.registerForm.patchValue({
      password: 'password123',
      confirmPassword: 'password123'
    });
    
    expect(component.registerForm.hasError('passwordMismatch')).toBeFalsy();
  });

  it('should call register service on valid form submission', () => {
    const mockResponse = { 
      token: 'test-token',
      expiration: new Date(),
      user: { id: '1', userName: 'testuser', email: 'test@example.com', createdAt: new Date() }
    };
    mockAuthService.register.and.returnValue(of(mockResponse));

    component.registerForm.patchValue({
      userName: 'testuser',
      email: 'test@example.com',
      firstName: 'Test',
      lastName: 'User',
      password: 'password123',
      confirmPassword: 'password123'
    });

    component.onSubmit();

    expect(mockAuthService.register).toHaveBeenCalled();
  });

  it('should navigate to login on successful registration', () => {
    const mockResponse = { 
      token: 'test-token',
      expiration: new Date(),
      user: { id: '1', userName: 'testuser', email: 'test@example.com', createdAt: new Date() }
    };
    mockAuthService.register.and.returnValue(of(mockResponse));

    component.registerForm.patchValue({
      userName: 'testuser',
      email: 'test@example.com',
      firstName: 'Test',
      lastName: 'User',
      password: 'password123',
      confirmPassword: 'password123'
    });

    component.onSubmit();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login']);
  });

  it('should handle registration error', () => {
    mockAuthService.register.and.returnValue(throwError(() => new Error('Registration failed')));

    component.registerForm.patchValue({
      userName: 'testuser',
      email: 'test@example.com',
      firstName: 'Test',
      lastName: 'User',
      password: 'password123',
      confirmPassword: 'password123'
    });

    component.onSubmit();

    expect(component.errorMessage).toBe('Registration failed. Please try again.');
    expect(component.isLoading).toBeFalsy();
  });
});
