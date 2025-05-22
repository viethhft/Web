import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
    { path: '', component: LoginComponent },
    { path: 'forgot-password', component: ForgotPasswordComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: '', redirectTo: 'login', pathMatch: 'full' }, // redirect /admin → /admin/login
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LoginRoutingModule { } 