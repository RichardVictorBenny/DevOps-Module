import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { LoginResponse, RefreshTokenBody, User } from '../models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  public get Token(): string | null {
    return localStorage.getItem('token');
  }

  public set Token(value: string | null) {
    if (value) {
      localStorage.setItem('token', value);
    } else {
      throw new Error('Token cannot be null');
    }
  }

  public RefreshToken(body: RefreshTokenBody): Observable<LoginResponse> {
    const endpoint = `${environment.apiUrl}/Authentication/Refresh`;
    return this.http.post<LoginResponse>(endpoint, body);
  }

  public GetCurrentUser(): Observable<User> {
    const endpoint = `${environment.apiUrl}/Authentication/CurrentUser`;
    return this.http.get<User>(endpoint);
  }
}
