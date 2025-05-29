import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) { }
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.getCookie('auth_token'); // tokenName là tên cookie chứa token
    let modifiedReq = request.clone({ withCredentials: true });

    if (token) {
      if (!request.headers.has('Authorization')) {
        modifiedReq = request.clone({
          withCredentials: true,
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        });
      }
    }
    return next.handle(modifiedReq);
  }

  getCookie(name: string): string | null {
    const ca: Array<string> = document.cookie.split(';');
    const cookieName = name + "=";
    for (let c of ca) {
      c = c.trim();
      if (c.indexOf(cookieName) === 0) return c.substring(cookieName.length, c.length);
    }
    return null;
  }

} 