import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { UserService } from '../../../../services/user/user.service';
import { GetList } from '../../../../share/Dtos/Dtos.Share';
import { ActionDto, UserDto } from '../../../../services/user/user.dtos';
import { BaseModel, DataSettingForm } from '../../../../share/Dtos/Base.model';
import { CookieService } from 'ngx-cookie-service';
import { Action } from 'rxjs/internal/scheduler/Action';

@Component({
    selector: 'app-employee-management',
    templateUrl: './employee-management.component.html',
    styleUrls: ['./employee-management.component.scss']
})
export class EmployeeManagementComponent extends BaseModel implements OnInit {
    searchQuery = "";
    selectedRole = "Tất cả vai trò";
    selectedStatus = "Tất cả trạng thái";
    currentPage = 1;
    totalPages = 1;
    dataGet: GetList = {
        PageNumber: 1,
        PageSize: 10
    };

    listUser: UserDto[] = [];

    roles = ["Tất cả vai trò", "Quản lý", "Nhân viên"];
    statuses = ["Tất cả trạng thái", "Đang hoạt động", "Không hoạt động"];

    constructor(
        private toastr: ToastrService,
        private userService: UserService,
        dialog: MatDialog,
        cookieService: CookieService,
        private cd: ChangeDetectorRef
    ) {
        super(dialog, cookieService);
    }

    ngOnInit(): void {
        this.getListUser(this.dataGet);
    }

    getListUser(data: GetList) {
        this.IsLoading;
        this.userService.getListUser(data).subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.listUser = response.data.data;
                    this.currentPage = response.data.currentPage;
                    this.totalPages = response.data.totalPage;
                    this.cd.detectChanges();
                    this.IsLoading = false;
                }
            }, (error) => {
                console.log(error);
                this.IsLoading = false;
            }
        );
    }

    getStatusDeleteClass(status: boolean): string {
        return status === false ? "status-active" : "status-inactive";
    }

    getStatusConfirmClass(status: boolean): string {
        return status === true ? "status-active" : "status-inactive";
    }

    onSearch(event: Event): void {
        this.searchQuery = (event.target as HTMLInputElement).value;
        this.toastr.info('Đang tìm kiếm...', 'Thông báo');
    }

    onRoleChange(event: Event): void {
        this.selectedRole = (event.target as HTMLSelectElement).value;
        this.toastr.info(`Đã lọc theo vai trò: ${this.selectedRole}`, 'Thông báo');
    }

    onStatusChange(event: Event): void {
        this.selectedStatus = (event.target as HTMLSelectElement).value;
        this.toastr.info(`Đã lọc theo trạng thái: ${this.selectedStatus}`, 'Thông báo');
    }

    previousPage(): void {
        if (this.CurrentPage > 1) {
            const page = this.CurrentPage - 1;
            this.goToPage(page);
        }
    }

    nextPage(): void {
        if (this.CurrentPage < this.TotalPage) {
            const page = this.CurrentPage + 1;
            this.goToPage(page);
        }
    }

    goToPage(page: number) {
        if (page >= 1 && page <= this.TotalPage && page !== this.CurrentPage) {
            this.IsLoading = true
            this.CurrentPage = page
            this.dataGet.PageNumber = page
            this.getListUser(this.dataGet)
        }
    }

    deleteEmployee(employee: UserDto): void {
        let action = new ActionDto(employee.id, this.getCurrentToken());
        this.userService.deleteUser(action).subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.toastr.success(response.message, 'Thông báo');
                    this.getListUser(this.dataGet);
                } else {
                    this.toastr.error(response.message || 'Có lỗi xảy ra khi xóa nhân viên', 'Lỗi');
                }
            }
            , (error) => {
                this.toastr.error('Có lỗi xảy ra khi xóa nhân viên', 'Lỗi');
                console.error('Error:', error);
            }
        );
    }

    activeEmployee(employee: UserDto): void {
        let action = new ActionDto(employee.id, this.getCurrentToken());
        this.userService.activateUser(action).subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.toastr.success(response.message, 'Thông báo');
                    this.getListUser(this.dataGet);
                } else {
                    this.toastr.error(response.message || 'Có lỗi xảy ra khi xóa nhân viên', 'Lỗi');
                }
            }
            , (error) => {
                this.toastr.error('Có lỗi xảy ra khi kích hoạt nhân viên', 'Lỗi');
                console.error('Error:', error);
            }
        );
    }

    addEmployee(): void {
        const data: DataSettingForm = {
            width: "600px",
            height: "400px",
            data: {
                title: "Thêm nhân viên mới",
                status: true,
            },
        };
        this.showDialog(AddEmployeeComponent, data).afterClosed().subscribe((result) => {
            if (result) {
                if (result.load) {
                    this.listUser = [];
                    this.dataGet.PageNumber = 1;
                    this.getListUser(this.dataGet);
                }
            } else {
                console.error("Lỗi khi thêm âm thanh mới");
            }
        });
    }
}
