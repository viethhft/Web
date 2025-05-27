import { ChangeDetectorRef, Component, Inject, OnInit } from "@angular/core"
import { NgForm } from "@angular/forms"
import { BaseModel } from "../../../../../share/Dtos/Base.model"
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { CreateUserDto } from "../../../../../services/user/user.dtos";
import { UserService } from "../../../../../services/user/user.service";
import { ToastrService } from 'ngx-toastr';
import { CookieService } from "ngx-cookie-service";

@Component({
    selector: 'app-add-employee',
    templateUrl: './add-employee.component.html',
    styleUrls: ['./add-employee.component.scss']
})
export class AddEmployeeComponent extends BaseModel implements OnInit {
    userDto: CreateUserDto = {
        displayName: "",
        email: "",
        gender: true,
        token: ""
    }
    constructor(
        private dialogRef: MatDialogRef<AddEmployeeComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any,
        private userService: UserService,
        private cd: ChangeDetectorRef,
        private toastr: ToastrService,
        cookieService: CookieService
    ) {
        super(undefined, cookieService);
    }

    ngOnInit(): void {
    }

    onSubmit(form: NgForm) {
        debugger
        form.form.markAllAsTouched();
        if (form.valid) {
            this.IsLoading = true;

            this.userDto.token = this.getCurrentToken();
            this.userService.createUser(this.userDto).subscribe(
                (response) => {
                    if (response.isSuccess) {
                        this.toastr.success('Thêm nhân viên thành công', 'Thông báo');
                        this.closeModal("Thêm nhân viên thành công", true)
                    } else {
                        this.toastr.error(response.message || 'Có lỗi xảy ra', 'Lỗi');
                    }
                },
                (error) => {
                    this.toastr.error('Có lỗi xảy ra khi thêm nhân viên', 'Lỗi');
                    console.error('Error:', error);
                }
            ).add(() => {
                this.IsLoading = false;
            });
        }
    }

    closeModal(message: string, load: boolean = false) {
        this.dialogRef.close({ message: message, load: load });
    }
}
