import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { LayoutService } from './shared/services/layout.service';
import { Router } from '@angular/router';
import { AuthService } from './authentication/services/auth.service';
import { of } from 'rxjs';

// Mock services
/**
 * @fileoverview Provides a mock implementation of the LayoutService for testing purposes.
 * @author Richard Benny
 * @purpose Used to simulate the LayoutService in unit tests, allowing control over the `isStandalonePage` observable.
 * @dependencies rxjs/of
 */
const mockLayoutService = {
  isStandalonePage: of(false),
};

const mockRouter = {
  navigate: jest.fn(),
};

const mockAuthService = {
  Logout: jest.fn(),
};

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        { provide: LayoutService, useValue: mockLayoutService },
        { provide: Router, useValue: mockRouter },
        { provide: AuthService, useValue: mockAuthService }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have the 'UI' title`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('UI');
  });

  it('should call router.navigate on login', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    app.onLogin();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/login'], { replaceUrl: true });
  });

  it('should call authService.Logout and set isLoggedIn to false on logout', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    app.isLoggedIn = true;
    app.onLogout();
    expect(app.isLoggedIn).toBe(false);
    expect(mockAuthService.Logout).toHaveBeenCalled();
  });

  it('should set isStandalonePage from layoutService', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    fixture.detectChanges();
    expect(app.isStandalonePage).toBe(false);
  });

  it('should correctly sum two numbers', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.sum(2, 3)).toBe(5);
  });

  it('should set user from localStorage if exists', () => {
    const testUser = { id: 1, name: 'Test User' };
    localStorage.setItem('user', JSON.stringify(testUser));
    
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    app.ngOnInit();

    expect(app.user).toEqual(testUser);
    expect(app.isLoggedIn).toBe(true);

    localStorage.removeItem('user'); 
  });

  it('should not set user if localStorage has no user', () => {
    localStorage.removeItem('user');
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    app.ngOnInit();

    expect(app.user).toBeNull();
    expect(app.isLoggedIn).toBe(false);
  });
});
