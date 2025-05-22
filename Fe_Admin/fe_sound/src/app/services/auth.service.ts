import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { LoginDto } from '../../services/user/user.dtos';
import { ResponseData } from '../../share/Dtos/Dtos.Share';
import { api } from '../../share/Environment/api.link';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  login(dataLogin: LoginDto): Observable<ResponseData<string>> {
    return this.http.post<ResponseData<string>>(api.user.login, dataLogin).pipe(
      tap(response => {
        if (response.data) {
          localStorage.setItem(this.TOKEN_KEY, response.data);
          this.isAuthenticatedSubject.next(true);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/admin/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  private hasToken(): boolean {
    return !!this.getToken();
  }
} 