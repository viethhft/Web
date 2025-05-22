import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FormsModule } from "@angular/forms"

import { LoginComponent } from './login.component';
import { LoginRoutingModule } from './login-routing.module';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { ShareModule } from '../../share/Component/share.module';

@NgModule({
  declarations: [
    LoginComponent,
    ForgotPasswordComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    FormsModule,
    LoginRoutingModule,
    DashboardModule,
    ShareModule
  ],
  exports: [
    LoginComponent,
    ForgotPasswordComponent
  ]
})
export class LoginModule { } 