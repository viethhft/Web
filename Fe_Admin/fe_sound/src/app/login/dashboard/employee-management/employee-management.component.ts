import { Component, OnInit } from "@angular/core"
import { ToastrService } from 'ngx-toastr'

interface Employee {
    id: number
    name: string
    email: string
    role: string
    status: string
}

@Component({
    selector: "app-employee-management",
    templateUrl: "./employee-management.component.html",
    styleUrls: ["./employee-management.component.scss"],
})
export class EmployeeManagementComponent implements OnInit {
    searchQuery = ""
    selectedRole = "Tất cả vai trò"
    selectedStatus = "Tất cả trạng thái"
    currentPage = 1
    totalPages = 1

    employees: Employee[] = [
        { id: 1, name: "Nguyễn Văn A", email: "nguyenvana@example.com", role: "Quản lý", status: "Đang hoạt động" },
        { id: 2, name: "Trần Thị B", email: "tranthib@example.com", role: "Nhân viên", status: "Đang hoạt động" },
        { id: 3, name: "Lê Văn C", email: "levanc@example.com", role: "Nhân viên", status: "Đang hoạt động" },
        { id: 4, name: "Phạm Thị D", email: "phamthid@example.com", role: "Nhân viên", status: "Không hoạt động" },
        { id: 5, name: "Hoàng Văn E", email: "hoangvane@example.com", role: "Nhân viên", status: "Đang hoạt động" },
        { id: 6, name: "Ngô Thị F", email: "ngothif@example.com", role: "Nhân viên", status: "Đang hoạt động" },
        { id: 7, name: "Đỗ Văn G", email: "dovang@example.com", role: "Nhân viên", status: "Không hoạt động" },
    ]

    roles = ["Tất cả vai trò", "Quản lý", "Nhân viên"]
    statuses = ["Tất cả trạng thái", "Đang hoạt động", "Không hoạt động"]

    constructor(private toastr: ToastrService) {}

    ngOnInit() {
        // Test toastr
        setTimeout(() => {
            this.toastr.success('Test message', 'Test title');
        }, 1000);
    }

    getStatusClass(status: string): string {
        return status === "Đang hoạt động" ? "status-active" : "status-inactive"
    }

    onSearch(event: Event): void {
        this.searchQuery = (event.target as HTMLInputElement).value
        this.toastr.info('Đang tìm kiếm...', 'Thông báo')
    }

    onRoleChange(event: Event): void {
        this.selectedRole = (event.target as HTMLSelectElement).value
        this.toastr.info(`Đã lọc theo vai trò: ${this.selectedRole}`, 'Thông báo')
    }

    onStatusChange(event: Event): void {
        this.selectedStatus = (event.target as HTMLSelectElement).value
        this.toastr.info(`Đã lọc theo trạng thái: ${this.selectedStatus}`, 'Thông báo')
    }

    previousPage(): void {
        if (this.currentPage > 1) {
            this.currentPage--
            this.toastr.info(`Đã chuyển đến trang ${this.currentPage}`, 'Thông báo')
        }
    }

    nextPage(): void {
        if (this.currentPage < this.totalPages) {
            this.currentPage++
            this.toastr.info(`Đã chuyển đến trang ${this.currentPage}`, 'Thông báo')
        }
    }

    editEmployee(employee: Employee): void {
        console.log(`Editing: ${employee.name}`)
        this.toastr.info(`Đang chỉnh sửa thông tin: ${employee.name}`, 'Thông báo')
    }

    deleteEmployee(employee: Employee): void {
        console.log(`Deleting: ${employee.name}`)
        this.toastr.warning(`Đã xóa nhân viên: ${employee.name}`, 'Cảnh báo')
    }

    addEmployee(): void {
        console.log("Adding new employee")
        this.toastr.success('Đã thêm nhân viên mới', 'Thành công')
    }
}
