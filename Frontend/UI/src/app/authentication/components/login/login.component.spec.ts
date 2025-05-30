import { TestBed, ComponentFixture } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { AuthService } from '../../services/auth.service';
import { FormService } from '../../../shared/services/form.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatModule } from '../../../shared/modules/mat.module';
import { CommonModule } from '@angular/common';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  /* eslint-disable @typescript-eslint/no-explicit-any */
  let mockAuthService: any;
  let mockFormService: any;
  let mockRouter: any;

  beforeEach(async () => {
    mockAuthService = {
      Login: jest.fn(),
      GetCurrentUser: jest.fn()
    };

    mockFormService = {
      Handle: jest.fn()
    };

    mockRouter = {
      navigate: jest.fn()
    };

    await TestBed.configureTestingModule({
      imports: [LoginComponent, ReactiveFormsModule, FormsModule, MatModule, CommonModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: FormService, useValue: mockFormService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component and initialize form', () => {
    expect(component).toBeTruthy();
    expect(component.loginForm).toBeDefined();
    expect(component.loginForm.controls['username']).toBeDefined();
    expect(component.loginForm.controls['password']).toBeDefined();
    expect(component.loginForm.controls['rememberMe']).toBeDefined();
  });

  it('should not submit if form is invalid', () => {
    component.loginForm.setValue({ username: '', password: '', rememberMe: false });
    component.onSubmit();

    expect(mockFormService.Handle).not.toHaveBeenCalled();
    expect(mockAuthService.Login).not.toHaveBeenCalled();
    expect(mockRouter.navigate).not.toHaveBeenCalled();
  });

  it('should call authService and navigate on successful login', done => {
    const fakeToken = 'fake-jwt-token';
    const fakeUser = { id: 1, name: 'Test User' };


    component.loginForm.setValue({ username: 'test@example.com', password: 'password', rememberMe: true });


    mockFormService.Handle.mockReturnValue(of(fakeToken));
    mockAuthService.Login.mockReturnValue(of(fakeToken));
    mockAuthService.GetCurrentUser.mockReturnValue(of(fakeUser));

    component.onSubmit();

    expect(mockFormService.Handle).toHaveBeenCalledWith(
      expect.anything(), 
      component.loginForm,
      'Login successful!'
    );

    // Because onSubmit triggers nested async calls
    setTimeout(() => {
      expect(localStorage.getItem('token')).toEqual(JSON.stringify(fakeToken));
      expect(localStorage.getItem('user')).toEqual(JSON.stringify(fakeUser));
      expect(mockRouter.navigate).toHaveBeenCalledWith(['/'], { replaceUrl: true });
      done();
    }, 0);
  });

  it('should navigate to register on onRegister call', () => {
    component.onRegister();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/register'], { replaceUrl: true });
  });
});
