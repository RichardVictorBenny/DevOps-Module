import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, Subject, switchMap, throwError } from 'rxjs';
import { AuthService } from '../authentication/services/auth.service';
import { environment } from '../../environments/environment';
import { LoginResponse } from '../authentication/models';
import { Router } from '@angular/router';

@Injectable()
export class AuthorizationInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshRequest$: Subject<LoginResponse> = new Subject<LoginResponse>();

  constructor(private authService: AuthService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const isApiUrl = request.url.startsWith(environment.apiUrl) &&
      !request.url.includes('refresh') &&
      !request.url.includes('login');

    const token = this.authService.Token;
    
    let loginResponse: LoginResponse | null = null;

    if(token) {
      loginResponse = JSON.parse(token) as LoginResponse;
      const isLoggedIn = loginResponse && loginResponse.accessToken;
      
      if (isLoggedIn && isApiUrl) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${loginResponse.accessToken}`
          }
        });
      }
    }



    // const authRequest = request.clone({
    //   headers: request.headers.set('Authorization', `Bearer ${token}`)
    // });

    return next.handle(request).pipe(
      catchError((error:HttpErrorResponse) => {
        if (error?.status === 401 && isApiUrl) {
          //handle refresh here
          return this.refreshTokenMethod(request, next, loginResponse);
        } else {
          return throwError(() => error);
        }
      })
    );
  }

  /* eslint-disable @typescript-eslint/no-explicit-any */
  public refreshTokenMethod(
    request: HttpRequest<any>,
    next: HttpHandler,
    loginResponse: LoginResponse | null = null
  ) : Observable<HttpEvent<any>> {

    if (!loginResponse) {
      this.LogoutAndRedirect();
      return throwError(() => "User is not logged in"); 
    }

    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.authService.RefreshToken(loginResponse).subscribe({
        next: res => {
          this.authService.Token = JSON.stringify(res);
          this.authService.GetCurrentUser().subscribe(user => {
            localStorage.setItem('user', JSON.stringify(user));
          });
          this.isRefreshing = false;
          this.refreshRequest$.next(res);
        },
        error: error => {
          this.isRefreshing = false;
          this.refreshRequest$.error(error);
        }
      });
    }

    return this.refreshRequest$.pipe(
      switchMap((res: LoginResponse) => {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${res.accessToken}`
          }
        });
        return next.handle(request);
      }),
      catchError((error) => {
        if (error.status === 401) {
          this.LogoutAndRedirect();
        }
        return throwError(() => error);
      })
    );
  }
  public LogoutAndRedirect() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.router.navigateByUrl('/login', { skipLocationChange: true }).catch((error) => {
      return throwError(() => error);
    });
  }
}
