import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { LoginComponent } from './login.component';
import { AuthService } from '../../../services/auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [LoginComponent, ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    mockAuthService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize login form with validators', () => {
    expect(component.loginForm.get('userName')?.hasError('required')).toBeTruthy();
    expect(component.loginForm.get('password')?.hasError('required')).toBeTruthy();
  });

  it('should validate required fields', () => {
    const userNameControl = component.loginForm.get('userName');
    const passwordControl = component.loginForm.get('password');

    userNameControl?.setValue('');
    passwordControl?.setValue('');
    
    expect(userNameControl?.invalid).toBeTruthy();
    expect(passwordControl?.invalid).toBeTruthy();
  });

  it('should call login service on valid form submission', () => {
    const mockResponse = { 
      token: 'test-token', 
      expiration: new Date(),
      user: { id: '1', userName: 'testuser', email: 'test@example.com', createdAt: new Date() } 
    };
    mockAuthService.login.and.returnValue(of(mockResponse));

    component.loginForm.patchValue({
      userName: 'testuser',
      password: 'password123'
    });

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalledWith({
      userName: 'testuser',
      password: 'password123'
    });
  });

  it('should navigate to feed on successful login', () => {
    const mockResponse = { 
      token: 'test-token', 
      expiration: new Date(),
      user: { id: '1', userName: 'testuser', email: 'test@example.com', createdAt: new Date() } 
    };
    mockAuthService.login.and.returnValue(of(mockResponse));

    component.loginForm.patchValue({
      userName: 'testuser',
      password: 'password123'
    });

    component.onSubmit();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/feed']);
  });

  it('should handle login error', () => {
    mockAuthService.login.and.returnValue(throwError(() => new Error('Login failed')));

    component.loginForm.patchValue({
      userName: 'testuser',
      password: 'wrongpassword'
    });

    component.onSubmit();

    expect(component.errorMessage).toBe('Invalid username or password');
    expect(component.isLoading).toBeFalsy();
  });

  it('should not submit invalid form', () => {
    component.loginForm.patchValue({
      userName: '',
      password: ''
    });

    component.onSubmit();

    expect(mockAuthService.login).not.toHaveBeenCalled();
  });
});
