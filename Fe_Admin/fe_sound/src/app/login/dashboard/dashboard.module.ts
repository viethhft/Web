import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { OverviewComponent } from './overview/overview.component';
import { EmployeeManagementComponent } from './employee-management/employee-management.component';
import { MusicManagementComponent } from './music-management/music-management.component';
import { SettingsModule } from './settings/settings.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { FormsModule } from '@angular/forms';

@NgModule({
    declarations: [
        DashboardComponent,
        OverviewComponent,
        EmployeeManagementComponent,
        MusicManagementComponent
    ],
    imports: [
        CommonModule,
        SettingsModule,
        DashboardRoutingModule,
        FormsModule
    ],
    exports: [
        DashboardComponent,
        OverviewComponent,
        EmployeeManagementComponent,
        MusicManagementComponent
    ]
})
export class DashboardModule { } 