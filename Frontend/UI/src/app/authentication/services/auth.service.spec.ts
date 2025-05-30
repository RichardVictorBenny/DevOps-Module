/**
 * File: auth.service.spec.ts
 * Author: Richard Benny
 * Purpose: Unit tests for the AuthService in the authentication module.
 * Dependencies: Angular TestBed, HttpClientTestingModule, Jest, Angular Router, environment, authentication models.
 *
 * This file contains a comprehensive suite of unit tests for the AuthService, verifying authentication logic such as token management,
 * login, registration, token refresh, user retrieval, and logout functionality. The tests use Angular's testing utilities and mock HTTP requests
 * to ensure the AuthService behaves as expected without requiring a real backend.
 */
import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse, RegisterRequest, RefreshTokenBody, User } from '../models';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
    /* eslint-disable @typescript-eslint/no-explicit-any */
  let mockRouter: any;

  beforeEach(() => {
    mockRouter = {
      navigate: jest.fn()
    };

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: Router, useValue: mockRouter }
      ]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('Token getter/setter', () => {
    it('should get and set token in localStorage', () => {
      service.Token = 'my-token';
      expect(service.Token).toBe('my-token');
    });

    it('should throw error if setting token to null', () => {
      expect(() => {
        service.Token = null;
      }).toThrow('Token cannot be null');
    });
  });

  describe('User getter', () => {
    it('should get user from localStorage', () => {
      localStorage.setItem('user', 'admin');
      expect(service.User).toBe('admin');
    });
  });

  describe('IsAuthenticated()', () => {
    it('should return true if token and user exist', () => {
      localStorage.setItem('token', 'valid-token');
      localStorage.setItem('user', 'user');
      expect(service.IsAuthenticated()).toBe(true);
    });

    it('should return false if token or user is missing', () => {
      localStorage.removeItem('token');
      expect(service.IsAuthenticated()).toBe(false);
    });
  });

  describe('Login()', () => {
    it('should call POST /Authentication/Login', () => {
      const loginData: LoginRequest = { username: 'test@example.com', password: 'pass' };
      const mockResponse: LoginResponse = { accessToken: 'abc', refreshToken: 'xyz', tokenType: 'Bearer', expiresIn: 3600 };

      service.Login(loginData).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/Authentication/Login`);
      expect(req.request.method).toBe('POST');
      req.flush(mockResponse);
    });
  });

  describe('Register()', () => {
    it('should call POST /Authentication/Register', () => {
      const registerData: RegisterRequest = {
        username: 'test@example.com',
        password: 'password',
        firstName: 'first',
        lastName: 'last'
      };

      service.Register(registerData).subscribe(response => {
        expect(response).toBeUndefined();
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/Authentication/Register`);
      expect(req.request.method).toBe('POST');
      req.flush(null);
    });
  });

  describe('RefreshToken()', () => {
    it('should call POST /Authentication/Refresh', () => {
      const tokenData: RefreshTokenBody = { refreshToken: 'ref-token' };
      const mockResponse: LoginResponse = { accessToken: 'new-token', refreshToken: 'new-ref', tokenType: 'Bearer', expiresIn: 3600 };

      service.RefreshToken(tokenData).subscribe(response => {
        expect(response).toEqual(mockResponse);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/Authentication/Refresh`);
      expect(req.request.method).toBe('POST');
      req.flush(mockResponse);
    });
  });

  describe('GetCurrentUser()', () => {
    it('should call GET /Authentication/CurrentUser', () => {
      const user: User = {
        UserId: 1, Email: 'admin@example.com',
        FirstName: 'Admin',
        LastName: 'One'
      };

      service.GetCurrentUser().subscribe(response => {
        expect(response).toEqual(user);
      });

      const req = httpMock.expectOne(`${environment.apiUrl}/Authentication/CurrentUser`);
      expect(req.request.method).toBe('GET');
      req.flush(user);
    });
  });

  describe('Logout()', () => {
    it('should clear localStorage and navigate to /login', () => {
      localStorage.setItem('token', 'abc');
      localStorage.setItem('user', 'admin');

      service.Logout();

      expect(localStorage.getItem('token')).toBeNull();
      expect(localStorage.getItem('user')).toBeNull();
      expect(mockRouter.navigate).toHaveBeenCalledWith(['/login'], { replaceUrl: true });
    });
  });
});
