import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeManagementComponent } from './employee-management.component';

const routes: Routes = [
    { path: 'dashboard', component: EmployeeManagementComponent },
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DashboardRoutingModule { } 