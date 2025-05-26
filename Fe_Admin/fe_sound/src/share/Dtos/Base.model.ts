
import { ComponentType } from "@angular/cdk/portal"
import { Injectable } from "@angular/core";
import { MatDialog, MatDialogRef } from "@angular/material/dialog"
import { CookieService } from "ngx-cookie-service";
import { jwtDecode, JwtPayload } from 'jwt-decode';

export interface DataSettingForm<T = any> {
    width?: string;
    height?: string;
    data?: T;
}
@Injectable({ providedIn: 'root' })
export class BaseModel {
    TotalPage: number = 0;
    CurrentPage: number = 0;
    PageSize: number = 0;
    IsLoading: boolean = false;
    TOKEN_KEY = 'auth_token';

    constructor(private dialog?: MatDialog, private cookieSer?: CookieService) {

    }
    showDialog<T, R = any>(
        component: ComponentType<T>,
        config?: DataSettingForm<R>
    ): MatDialogRef<T, R> {
        return this.dialog!.open<T, R>(component, {
            width: config?.width ?? '600px',
            height: config?.height ?? '400px',
            data: config?.data ?? null,
        });
    }

    getCurrentToken(): string {
        try {
            const token = this.cookieSer!.get(this.TOKEN_KEY);

            if (!token) return "";

            const payload = jwtDecode<JwtPayload>(token);

            let test = Date.now() / 1000;
            if (payload.exp && payload.exp < Date.now() / 1000) {
                return "";
            }

            return token;
        } catch (error) {
            console.error("Token decode error:", error);
            return "";
        }
    }

}
