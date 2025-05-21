import { Component } from "@angular/core"
import { Router } from "@angular/router"

@Component({
    selector: "app-employee-management",
    templateUrl: 'employee-management.component.html',
    styleUrls: ["./employee-management.component.scss"],
})
export class EmployeeManagementComponent {
    activeTab = "overview"

    constructor(private router: Router) { }

    setActiveTab(tab: string) {
        this.activeTab = tab
    }

    logout() {
        this.router.navigate(["/"])
    }

    getSoundWaveHeight(index: number): number {
        return Math.sin(index / 5) * 8 + 10
    }
}
