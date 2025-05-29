import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginResponse, RefreshTokenBody, RegisterRequest, User } from '../models';
import { Observable } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  public get Token(): string | null {
    return localStorage.getItem('token');
  }

  public get User(): string | null {
    return localStorage.getItem('user');
  }

  // eslint-disable-next-line @typescript-eslint/adjacent-overload-signatures
  public set Token(value: string | null) {
    if (value) {
      localStorage.setItem('token', value);
    } else {
      throw new Error('Token cannot be null');
    }
  }

  public IsAuthenticated(): boolean {
    return this.Token !== null && this.User !== null;
  }

  public RefreshToken(body: RefreshTokenBody): Observable<LoginResponse> {
    console.log('Refreshing token...', body);
    const endpoint = `${environment.apiUrl}/Authentication/Refresh`;
    return this.http.post<LoginResponse>(endpoint, {refreshToken: body.refreshToken});
  }

  public GetCurrentUser(): Observable<User> {
    const endpoint = `${environment.apiUrl}/Authentication/CurrentUser`;
    return this.http.get<User>(endpoint);
  }

  public Login(body: LoginRequest): Observable<LoginResponse> {
    const endpoint = `${environment.apiUrl}/Authentication/Login`;
    return this.http.post<LoginResponse>(endpoint, body);
  }
  
  public Register(body: RegisterRequest): Observable<void> {
    const endpoint = `${environment.apiUrl}/Authentication/Register`;
    return this.http.post<void>(endpoint, body);
  }

  public Logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }
}
