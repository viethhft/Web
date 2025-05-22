import { Component } from "@angular/core"
import { LoginDto } from "../../services/user/user.dtos"
import { AuthService } from "../../app/services/auth.service"
import { NgForm } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { Router } from "@angular/router";

@Component({
    selector: "app-login",
    templateUrl: './login.component.html',
    styleUrls: ["./login.component.scss"],
})
export class LoginComponent {
    userLogin: LoginDto = {
        name: null,
        password: null,
    }
    isLoading = false
    showPassword = false
    errorMessage?: string;

    constructor(private authService: AuthService, private cookieService: CookieService, private router: Router) { }

    onSubmit(form: NgForm) {
        if (form.valid) {
            this.isLoading = true;
            this.authService.login(this.userLogin).subscribe(
                (response) => {
                    this.isLoading = false;
                    if (response.isSuccess) {
                        console.log("Login successful", response);
                        alert(response.message);
                        this.cookieService.set('tk', response.data, 1);
                        this.errorMessage = undefined;
                        // Navigate to dashboard with the correct path
                        this.router.navigate(['/admin/dashboard']);
                    }
                    else {
                        this.errorMessage = response.message;
                    }
                },
                (error) => {
                    this.isLoading = false;
                    console.error("Login failed", error);
                }
            );
        }
        else {
            this.errorMessage = "Vui lòng nhập tên đăng nhập và mật khẩu";
        }
    }

    togglePasswordVisibility() {
        this.showPassword = !this.showPassword
    }
}
