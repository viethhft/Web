import { ChangeDetectorRef, Component, OnInit, ViewContainerRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { UserService } from '../../../../services/user/user.service';
import { GetList, GetListFilterRole, GetListFilterStatus, GetListSearch } from '../../../../share/Dtos/Dtos.Share';
import { ActionDto, RoleDto, UserDto } from '../../../../services/user/user.dtos';
import { BaseModel, DataSettingForm } from '../../../../share/Dtos/Base.model';
import { CookieService } from 'ngx-cookie-service';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { EditRoleComponent } from './edit-role/edit-role.component';

@Component({
    selector: 'app-employee-management',
    templateUrl: './employee-management.component.html',
    styleUrls: ['./employee-management.component.scss']
})
export class EmployeeManagementComponent extends BaseModel implements OnInit {
    searchQuery = "";
    selectedRole = "";
    selectedStatus = "";

    dataGet: GetList = {
        PageNumber: 1,
        PageSize: 10
    };

    dataSearch: GetListSearch = {
        PageNumber: 1,
        PageSize: 10,
        Key: ""
    };

    dataFilterRole: GetListFilterRole = {
        PageNumber: 1,
        PageSize: 10,
        Role: ""
    };

    dataFilterStatus: GetListFilterStatus = {
        PageNumber: 1,
        PageSize: 10,
        Status: false
    };
    searchTime: any;
    private overlayRef: OverlayRef | null = null;
    listUser: UserDto[] = [];

    roles: RoleDto[] = [];
    statuses = [
        { label: 'Tất cả trạng thái', value: null },
        { label: 'Hoạt động', value: false },
        { label: 'Không hoạt động', value: true }
    ];

    constructor(
        private toastr: ToastrService,
        private userService: UserService,
        dialog: MatDialog,
        cookieService: CookieService,
        private cd: ChangeDetectorRef,
        private overlay: Overlay,
    ) {
        super(dialog, cookieService);
    }

    ngOnInit(): void {
        this.getListUser(this.dataGet);
        this.userService.getListRole().subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.roles = [{ id: '', name: 'Tất cả vai trò' }, ...response.data];
                    this.cd.detectChanges();
                }
            },
            (error) => {
                console.log(error);
            }
        )
    }

    getListUser(data: GetList) {
        this.IsLoading;
        this.userService.getListUser(data).subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.listUser = response.data.data;
                    this.CurrentPage = response.data.currentPage;
                    this.TotalPage = response.data.totalPage;
                    this.cd.detectChanges();
                    this.IsLoading = false;
                }
            }, (error) => {
                console.log(error);
                this.IsLoading = false;
            }
        );
    }

    resetFilter() {
        this.searchQuery = '';
        this.selectedRole = '';
        this.selectedStatus = 'null';
    }

    editRole(event: Event, employee: UserDto): void {
        if (this.overlayRef) {
            this.overlayRef.dispose();
        }

        const target = event.target as HTMLElement;
        const positionStrategy = this.overlay
            .position()
            .flexibleConnectedTo(target)
            .withPositions([
                {
                    originX: 'end',
                    originY: 'center',
                    overlayX: 'start',
                    overlayY: 'center',
                },
            ]);

        this.overlayRef = this.overlay.create({
            positionStrategy,
            hasBackdrop: true,
            backdropClass: 'transparent-backdrop',
            scrollStrategy: this.overlay.scrollStrategies.close(),
        });
        const portal = new ComponentPortal(EditRoleComponent);
        const componentRef = this.overlayRef.attach(portal);

        componentRef.instance.data = { ...employee };
        componentRef.instance.close.subscribe(() => {
            this.overlayRef?.dispose();
        });
        // Click ra ngoài sẽ đóng
        this.overlayRef.backdropClick().subscribe(() => this.overlayRef?.dispose());

    }

    getStatusDeleteClass(status: boolean): string {
        return status === false ? "status-active" : "status-inactive";
    }

    getStatusConfirmClass(status: boolean): string {
        return status === true ? "status-active" : "status-inactive";
    }

    onSearch(event: Event): void {
        this.resetFilter();
        this.searchQuery = (event.target as HTMLInputElement).value;
        if (this.searchQuery === "") {
            this.dataGet.PageNumber = 1;
            this.getListUser(this.dataGet);
            return;
        }
        this.toastr.info('Đang tìm kiếm...', 'Thông báo');
        if (this.searchTime) {
            clearTimeout(this.searchTime);
        }
        this.searchTime = setTimeout(() => {
            this.dataSearch.Key = this.searchQuery;
            this.userService.searchUser(this.dataSearch).subscribe(
                (response) => {
                    this.listUser = response.data.data;
                    this.CurrentPage = response.data.currentPage;
                    this.TotalPage = response.data.totalPage;
                    this.toastr.info("Tìm kiếm hoàn tất");
                    this.cd.detectChanges();
                },
                (error) => {
                    console.log(error);
                    this.toastr.error("Có lỗi xảy ra vui lòng liên hệ nhà phát triển")
                }
            )
        }, 300);
    }

    onRoleChange(event: Event): void {
        this.resetFilter();
        this.selectedRole = (event.target as HTMLSelectElement).value;
        const namerole = this.roles.filter(c => c.id === this.selectedRole)[0];

        this.toastr.info(`Đang lọc theo vai trò ${namerole.name}`, 'Thông báo');

        if (this.selectedRole === '') {
            this.dataGet.PageNumber = 1;
            this.getListUser(this.dataGet);
            return;
        }

        this.dataFilterRole.Role = this.selectedRole;
        this.userService.filterUserByRole(this.dataFilterRole).subscribe(
            (response) => {
                this.listUser = response.data.data;
                this.CurrentPage = response.data.currentPage;
                this.TotalPage = response.data.totalPage;
                this.toastr.info("Lọc hoàn tất");
                this.cd.detectChanges();
            },
            (error) => {
                console.log(error);
            }
        )
    }

    onStatusChange(event: Event): void {
        this.resetFilter();
        const value = (event.target as HTMLSelectElement).value;
        this.selectedStatus = value;
        console.log(this.selectedStatus);

        const status = this.statuses.filter(c => (c.value !== null ? c.value.toString() : 'null') === this.selectedStatus)[0];
        this.toastr.info(`Đã lọc theo trạng thái: ${status.label}`, 'Thông báo');
        if (status.value !== null) {
            this.dataFilterStatus.Status = status.value ?? false;
        }
        else {
            this.dataGet.PageNumber = 1;
            this.getListUser(this.dataGet);
            return;
        }
        this.userService.filterUserByStatus(this.dataFilterStatus).subscribe(
            (response) => {
                this.listUser = response.data.data;
                this.CurrentPage = response.data.currentPage;
                this.TotalPage = response.data.totalPage;
                this.toastr.info("Lọc hoàn tất");
                this.cd.detectChanges();
            },
            (error) => {
                console.log(error);
            }
        )
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
