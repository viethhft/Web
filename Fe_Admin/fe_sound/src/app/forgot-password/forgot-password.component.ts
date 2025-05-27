import { Component, OnInit } from "@angular/core"
import { FormBuilder, FormGroup, NgForm, Validators } from "@angular/forms"
import { UserService } from "../../services/user/user.service"
import { ToastrService } from "ngx-toastr"
import { ForgotPasswordDto } from "../../services/user/user.dtos"

@Component({
    selector: "app-forgot-password",
    templateUrl: './forgot-password.component.html',
    styleUrls: ["./forgot-password.component.scss"],
})
export class ForgotPasswordComponent implements OnInit {
    forgotPasswordForm: FormGroup
    isLoading = false;
    emailSent = false;
    errorMessage?: string;
    forgotPasswordDto: ForgotPasswordDto = {
        email: "",
        code: "",
        newPassword: "",
        confirmPassword: "",

    };
    constructor(private fb: FormBuilder, private userService: UserService, private toastrService: ToastrService) {
        this.forgotPasswordForm = this.fb.group({
            email: ["", [Validators.required, Validators.email]],
        })
    }

    ngOnInit(): void {
        if (localStorage.getItem('emailSent')?.length) {
            this.emailSent = true;
        }
    }

    onSubmitSendCode() {
        if (this.forgotPasswordForm.valid) {
            this.isLoading = true;
            this.userService.forgotPassword(this.forgotPasswordForm.value.email).subscribe(
                (response) => {
                    this.isLoading = false;
                    if (response.isSuccess) {
                        this.emailSent = true;
                        localStorage.setItem('emailSent', this.forgotPasswordForm.value.email);
                        this.toastrService.success(response.message);
                    } else {
                        console.error("Error sending email:", response.message);
                        this.errorMessage = response.message;
                        this.toastrService.error(response.message);
                    }
                },
                (error) => {
                    this.isLoading = false;
                    console.error("Error sending email:", error);
                }
            );
        }
    }
    onSubmitCode(form: NgForm) {
        form.form.markAllAsTouched();
        if (form.form.valid) {
            this.isLoading = true;
            this.forgotPasswordDto.email = localStorage.getItem('emailSent') || '';

            this.userService.changeForgotPassword(this.forgotPasswordDto).subscribe(
                (response) => {
                    this.isLoading = false;
                    this.errorMessage = undefined;
                    if (response.isSuccess) {
                        this.toastrService.success(response.message);
                        let time = 1;
                        for (let i = 5; i >= 0; i--) {
                            setTimeout(() => {
                                this.toastrService.info(`Bạn sẽ được chuyển hướng đến trang đăng nhập trong ${i} giây.`);
                            }, time * 1000);
                            time++;
                        }
                        localStorage.removeItem('emailSent');
                        setTimeout(() => {
                            this.emailSent = false;
                            window.location.href = '/admin/login';
                        }, 5000);
                    } else {
                        console.error("Error confirming code:", response.message);
                        this.errorMessage = response.message;
                        this.toastrService.error(response.message);
                    }
                },
                (error) => {
                    this.isLoading = false;
                    console.error("Error confirming code:", error);
                    this.errorMessage = "Đã xảy ra lỗi khi xác nhận mã. Vui lòng thử lại.";
                    this.toastrService.error("Đã xảy ra lỗi khi xác nhận mã. Vui lòng thử lại.");
                });
        }
    }
}
