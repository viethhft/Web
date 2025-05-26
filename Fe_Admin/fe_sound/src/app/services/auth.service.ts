import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, tap } from 'rxjs';
import { Router } from '@angular/router';
import { LoginDto } from '../../services/user/user.dtos';
import { ResponseData } from '../../share/Dtos/Dtos.Share';
import { api } from '../../share/Environment/api.link';
import { CookieService } from 'ngx-cookie-service';
import { jwtDecode } from 'jwt-decode';
import { map } from 'rxjs/operators';
import { JwtPayload } from "jwt-decode";

interface MyJwtPayload extends JwtPayload {
  DisplayName?: string;
  IsConfirm?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenSubject: BehaviorSubject<string | null>;
  private readonly TOKEN_KEY = 'auth_token';

  constructor(
    private http: HttpClient,
    private router: Router,
    private cookieService: CookieService,
  ) {
    const initialToken = this.cookieService.get(this.TOKEN_KEY) || null;
    this.tokenSubject = new BehaviorSubject<string | null>(initialToken);
  }

  login(dataLogin: LoginDto): Observable<ResponseData<string>> {
    return this.http.post<ResponseData<string>>(api.user.login, dataLogin).pipe(
      tap(response => {
        if (response.isSuccess) {
          let token = jwtDecode<MyJwtPayload>(response.data);
          localStorage.setItem('name', token.DisplayName || '');
          localStorage.setItem('isConfirm', (token.IsConfirm ?? false).toString());
          this.cookieService.set(this.TOKEN_KEY, response.data);
          this.tokenSubject.next(response.data);
        }
      })
    );
  }

  logout(): void {
    this.cookieService.delete(this.TOKEN_KEY);
    localStorage.removeItem('name');
    localStorage.removeItem('isConfirm');
    this.tokenSubject.next(null);
    this.router.navigate(['/admin/login']);
  }

  getToken(): Observable<string | null> {
    return this.tokenSubject.asObservable();
  }

  isAuthenticated(): Observable<boolean> {
    return this.tokenSubject.asObservable().pipe(
      map(token => {
        if (!token) {
          this.logout();
          return false;
        }

        try {
          const data = jwtDecode<MyJwtPayload>(token);
          const now = Math.floor(Date.now() / 1000);

          if (data.exp && data.exp < now) {
            this.logout();
            return false;
          }

          return true;
        } catch (err) {
          this.logout();
          return false;
        }
      })
    );
  }
}
