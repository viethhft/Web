import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { AuthService } from "../../services/auth.service"
@Component({
    selector: "app-dashboard",
    templateUrl: "./dashboard.component.html",
    styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent implements OnInit {

    activeTab = "analytics"
    name: string = '';
    constructor(private authService: AuthService) {

    }
    ngOnInit(): void {
        this.name = localStorage.getItem("name") || "";
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
