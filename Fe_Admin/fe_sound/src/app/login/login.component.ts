import { Component, OnInit } from "@angular/core"
import { LoginDto } from "../../services/user/user.dtos"
import { AuthService } from "../services/auth.service"
import { NgForm } from '@angular/forms';
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { BaseModel } from "../../share/Dtos/Base.model";

@Component({
    selector: "app-login",
    templateUrl: './login.component.html',
    styleUrls: ["./login.component.scss"],
})
export class LoginComponent extends BaseModel implements OnInit {
    userLogin: LoginDto = {
        name: null,
        password: null,
    }
    showPassword = false
    errorMessage?: string;

    constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {
        super()
    }

    ngOnInit(): void {
        if (this.authService.isAuthenticated()) {
            this.router.navigate(['/admin/dashboard']);
        }
    }

    onSubmit(form: NgForm) {
        if (form.valid) {
            this.IsLoading = true;
            this.authService.login(this.userLogin).subscribe(
                (response) => {
                    this.IsLoading = false;
                    if (response.isSuccess) {
                        this.toastr.success(response.message);
                        this.errorMessage = undefined;
                        window.location.href = '/admin/dashboard';
                    }
                    else {
                        this.errorMessage = response.message;
                    }
                },
                (error) => {
                    this.IsLoading = false;
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
