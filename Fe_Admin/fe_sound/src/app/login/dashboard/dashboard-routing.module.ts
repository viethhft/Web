import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { OverviewComponent } from './overview/overview.component';
import { SettingsComponent } from './settings/settings.component';
import { MusicManagementComponent } from './music-management/music-management.component';
import { EmployeeManagementComponent } from './employee-management/employee-management.component';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        children: [
            { path: '', redirectTo: 'overview', pathMatch: 'full' },
            { path: 'overview', component: OverviewComponent },
            { path: 'settings', component: SettingsComponent },
            { path: 'music-management', component: MusicManagementComponent },
            { path: 'employee-management', component: EmployeeManagementComponent },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DashboardRoutingModule { } 