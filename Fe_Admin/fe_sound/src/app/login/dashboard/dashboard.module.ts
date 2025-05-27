import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { OverviewComponent } from './overview/overview.component';
import { EmployeeManagementComponent } from './employee-management/employee-management.component';
import { SettingsModule } from './settings/settings.module';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MusicManagementModule } from './music-management/music-management.module';
import { EmployeeManagementModule } from './employee-management/employee-management.module';

@NgModule({
    declarations: [
        DashboardComponent,
        OverviewComponent,
    ],
    imports: [
        CommonModule,
        SettingsModule,
        DashboardRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        MusicManagementModule,
        EmployeeManagementModule
    ],
    exports: [
        DashboardComponent,
        OverviewComponent,
    ]
})
export class DashboardModule { } 