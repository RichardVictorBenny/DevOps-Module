/**
 * File: authorization.interceptor.spec.ts
 * Author: Richard Benny
 * Purpose: Unit tests for the AuthorizationInterceptor, which handles adding authorization headers,
 *          refreshing tokens on 401 errors, and redirecting to login when unauthorized.
 * Dependencies: Angular TestBed, Jest, RxJS, Angular Router, HttpClient, AuthService, environment
 *
 * This test suite verifies:
 * - The interceptor is created successfully.
 * - It adds an Authorization header to API requests when a token exists.
 * - It attempts to refresh the token and retries the request on a 401 Unauthorized error.
 * - It logs out and redirects to the login page if no token is present when a 401 occurs.
 */
import { TestBed } from '@angular/core/testing';
import { AuthorizationInterceptor } from './authorization.interceptor';
import { AuthService } from '../authentication/services/auth.service';
import { Router } from '@angular/router';
import { HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { of, throwError, Subject } from 'rxjs';
import { environment } from '../../environments/environment';

describe('AuthorizationInterceptor', () => {
  let interceptor: AuthorizationInterceptor;
  let authServiceMock: jest.Mocked<AuthService>;
  let routerMock: jest.Mocked<Router>;
  let httpHandlerMock: jest.Mocked<HttpHandler>;

    /* eslint-disable @typescript-eslint/no-explicit-any */
  beforeEach(() => {
    authServiceMock = {
      Token: '',
      RefreshToken: jest.fn(),
      GetCurrentUser: jest.fn()
    } as any;

    routerMock = {
      navigateByUrl: jest.fn().mockResolvedValue(true)
    } as any;

    httpHandlerMock = {
      handle: jest.fn()
    };

    TestBed.configureTestingModule({
      providers: [
        AuthorizationInterceptor,
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    });

    interceptor = TestBed.inject(AuthorizationInterceptor);
  });

  it('should be created', () => {
    expect(interceptor).toBeTruthy();
  });

  it('should add Authorization header if token exists and request is to API', (done) => {
    const loginResponse = { accessToken: 'token123', refreshToken: 'refresh123' };
    authServiceMock.Token = JSON.stringify(loginResponse);

    const request = new HttpRequest('GET', `${environment.apiUrl}/data`);
    const nextHandle = new Subject<HttpEvent<any>>();
    httpHandlerMock.handle.mockReturnValue(nextHandle.asObservable());

    interceptor.intercept(request, httpHandlerMock).subscribe(event => {
      expect(event).toBeTruthy();
      done();
    });

    const interceptedRequest = httpHandlerMock.handle.mock.calls[0][0] as HttpRequest<any>;
    expect(interceptedRequest.headers.get('Authorization')).toBe(`Bearer ${loginResponse.accessToken}`);

    nextHandle.next(new HttpResponse({ status: 200 }));
    nextHandle.complete();
  });

  it('should call refreshTokenMethod on 401 error and retry request after refresh', (done) => {
    const loginResponse = { accessToken: 'old-token', refreshToken: 'refresh123' };
    authServiceMock.Token = JSON.stringify(loginResponse);

    const request = new HttpRequest('GET', `${environment.apiUrl}/data`);
    const errorResponse = new HttpErrorResponse({ status: 401, url: request.url });

    const refreshResponse = { accessToken: 'new-token', refreshToken: 'new-refresh' };
    const refreshedUser = { UserId: 1, Email: 'user@example.com', FirstName: 'User', LastName: 'Test' };

    // Setup RefreshToken to emit new token
    const refreshSubject = new Subject<any>();
    authServiceMock.RefreshToken.mockReturnValue(refreshSubject.asObservable());
    authServiceMock.GetCurrentUser.mockReturnValue(of(refreshedUser));

    // First handle returns 401 error
    httpHandlerMock.handle.mockReturnValueOnce(throwError(() => errorResponse));
    // Second handle returns success after refresh
    httpHandlerMock.handle.mockReturnValueOnce(of(new HttpResponse({ status: 200 })));

    interceptor.intercept(request, httpHandlerMock).subscribe({
      next: (event) => {
        expect(event).toBeTruthy();
        expect(authServiceMock.Token).toBe(JSON.stringify(refreshResponse));
        expect(authServiceMock.GetCurrentUser).toHaveBeenCalled();
        done();
      },
      error: () => {
        done.fail('Should not error');
      }
    });

    // Trigger refresh token success
    refreshSubject.next(refreshResponse);
    refreshSubject.complete();

    // Verify that handle was called twice (first error, then retry)
    expect(httpHandlerMock.handle).toHaveBeenCalledTimes(2);
  });

  

  it('should logout and redirect if no loginResponse when 401 received', (done) => {
    authServiceMock.Token = null;

    const request = new HttpRequest('GET', `${environment.apiUrl}/data`);
    const errorResponse = new HttpErrorResponse({ status: 401, url: request.url });

    httpHandlerMock.handle.mockReturnValueOnce(throwError(() => errorResponse));

    interceptor.intercept(request, httpHandlerMock).subscribe({
      next: () => done.fail('Expected error'),
      error: () => {
        expect(routerMock.navigateByUrl).toHaveBeenCalledWith('/login', { skipLocationChange: true });
        done();
      }
    });
  });

});
