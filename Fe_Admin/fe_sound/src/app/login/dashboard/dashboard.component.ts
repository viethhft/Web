import { Component } from "@angular/core"
import { Router } from "@angular/router"

@Component({
    selector: "app-dashboard",
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent {
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
