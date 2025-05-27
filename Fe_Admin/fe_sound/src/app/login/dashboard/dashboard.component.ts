import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { AuthService } from "../../services/auth.service"
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { BaseModel } from "../../../share/Dtos/Base.model";
import { UserService } from "../../../services/user/user.service";
import { VerifyFirstLogInDto } from "../../../services/user/user.dtos";
import { ToastrService } from "ngx-toastr";
import { CookieService } from "ngx-cookie-service";
@Component({
    selector: "app-dashboard",
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent extends BaseModel implements OnInit {

    activeTab = "analytics"
    name: string = '';
    isConfirm: boolean = false;
    firstLoginForm: FormGroup;
    errorMessage: string = '';
    constructor(
        private authService: AuthService, private userService: UserService,
        private fb: FormBuilder, private toastrSer: ToastrService,
        cookieSer: CookieService
    ) {
        super(undefined, cookieSer);
        this.firstLoginForm = this.fb.group({
            codeFirstLogin: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(6)]],
        });
    }
    ngOnInit(): void {
        this.name = localStorage.getItem("name") || "";
        let check = localStorage.getItem("isConfirm");
        if (check) {
            this.isConfirm = check.toLowerCase() === "true";
        }
        else {
            this.logout();
        }
    }

    onSubmitSendCodeFirstLogin() {
        this.firstLoginForm.markAllAsTouched();
        if (this.firstLoginForm.invalid)
            return;
        this.userService.verifyFirstLogIn(new VerifyFirstLogInDto(this.firstLoginForm.get('codeFirstLogin')?.value
            , this.getCurrentToken())).subscribe({
                next: (response) => {
                    if (response.isSuccess) {
                        localStorage.setItem("isConfirm", "true");
                        this.toastrSer.success(response.message || "Code verified successfully.");
                        this.isConfirm = true;
                        this.errorMessage = '';
                        setTimeout(() => {
                            window.location.reload();
                        }, 5000);
                    } else {
                        this.toastrSer.error(response.message || "Failed to verify code.");
                    }
                },
                error: (error) => {
                    this.errorMessage = error.error.message || "An error occurred while sending the code.";
                    console.log(error);
                }
            });
    }

    setActiveTab(tab: string) {
        this.activeTab = tab
    }

    logout() {
        this.authService.logout();
    }

    getSoundWaveHeight(index: number): number {
        return Math.sin(index / 5) * 8 + 10
    }
}
