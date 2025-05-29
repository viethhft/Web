import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { OverviewComponent } from './overview/overview.component';
import { SettingsComponent } from './settings/settings.component';
import { MusicManagementComponent } from './music-management/music-management.component';
import { EmployeeManagementComponent } from './employee-management/employee-management.component';
import { AuthGuard } from '../../guards/auth.guard';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        children: [
            { path: '', redirectTo: 'overview', pathMatch: 'full' },
            { path: 'overview', component: OverviewComponent, canActivate: [AuthGuard], data: { roles: ['STAFF', 'ADMIN'] } },
            { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard], data: { roles: ['STAFF', 'ADMIN'] } },
            { path: 'music-management', component: MusicManagementComponent, canActivate: [AuthGuard], data: { roles: ['STAFF', 'ADMIN'] } },
            { path: 'employee-management', component: EmployeeManagementComponent, canActivate: [AuthGuard], data: { roles: ['ADMIN'] } },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DashboardRoutingModule { } 