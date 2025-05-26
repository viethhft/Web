import { Component } from "@angular/core"
import { Form, FormBuilder, FormGroup, NgForm } from "@angular/forms"
import { UserService } from "../../../../services/user/user.service"
import { ChangePasswordDto, UpdateInfoDto } from "../../../../services/user/user.dtos"
import { ToastrService } from "ngx-toastr"
import { BaseModel } from "../../../../share/Dtos/Base.model"
import { CookieService } from "ngx-cookie-service"

@Component({
    selector: "app-settings",
    templateUrl: './settings.component.html',
    styleUrls: ["./settings.component.scss"],
})
export class SettingsComponent extends BaseModel {
    userUpdateForm: UpdateInfoDto = {
        email: '',
        name: '',
        phoneNumber: '',
        displayName: '',
        token: '',
    }
    changePasswordForm: ChangePasswordDto = {
        token: '',
        oldPassword: '',
        newPassword: '',
        confirmPassword: '',
    }
    notificationSettings = {
        emailNotifications: true,
        systemNotifications: true,
        updateNotifications: false,
    }
    systemSettings = {
        darkMode: true,
    }

    constructor(private userService: UserService, private toastr: ToastrService, private cookieService: CookieService
    ) {
        super(undefined, cookieService);
        this.loadUserInfo();

    }
    loadUserInfo() {
        this.userService.getProfile(this.getCurrentToken()).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.userUpdateForm = {
                        email: response.data.email,
                        name: response.data.name,
                        phoneNumber: response.data.phoneNumber ? response.data.phoneNumber : 'Chưa cập nhật',
                        displayName: response.data.displayName,
                        token: this.getCurrentToken(),
                    };
                } else {
                    this.toastr.error(response.message, "Thông báo");
                }
            }
            , error: (error) => {

                console.error("Error loading user info", error);
                this.toastr.error("Không thể tải thông tin người dùng", "Thông báo");
            }
        });
    }

    saveAccountChanges(form: NgForm) {
        this.userUpdateForm.token = this.getCurrentToken();
        this.userService.updateUser(this.userUpdateForm).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.toastr.success(response.message, "Thông báo");
                } else {
                    this.toastr.error(response.message, "Thông báo");
                }
            }
            , error: (error) => {
                console.error("Error saving account changes", error);
            }
        });
    }

    changePassword(form: NgForm) {
        form.form.markAllAsTouched();
        if (form.form.valid) {
            if (this.changePasswordForm.newPassword !== this.changePasswordForm.confirmPassword) {
                this.toastr.error("Mật khẩu mới và xác nhận mật khẩu không khớp", "Thông báo");
                return;
            }
            this.changePasswordForm.token = this.getCurrentToken();
            this.userService.changePassword(this.changePasswordForm).subscribe({
                next: (response) => {

                    if (response.isSuccess) {
                        this.toastr.success(response.message, "Thông báo");
                        this.changePasswordForm = {
                            token: this.getCurrentToken(),
                            oldPassword: '',
                            newPassword: '',
                            confirmPassword: '',
                        };
                    } else {
                        this.toastr.error(response.message, "Thông báo");
                    }
                }
                , error: (error) => {
                    console.error("Error changing password", error);
                    this.toastr.error("Không thể thay đổi mật khẩu", "Thông báo");
                }
            });
        }
    }

    toggleNotification(setting: keyof typeof this.notificationSettings): void {
        this.notificationSettings[setting] = !this.notificationSettings[setting]
    }

    toggleDarkMode(): void {
        this.systemSettings.darkMode = !this.systemSettings.darkMode
    }

    saveSystemChanges(): void {
        console.log("Saving system changes")
        // Implement save logic here
    }
}
