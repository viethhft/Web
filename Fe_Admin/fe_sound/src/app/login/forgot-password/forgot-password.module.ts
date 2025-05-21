import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ForgotPasswordComponent } from './forgot-password.component';
import { ForgotPasswordRoutingModule } from './forgot-password-routing.module';

@NgModule({
    declarations: [
        ForgotPasswordComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        ForgotPasswordRoutingModule
    ],
    exports: [
        ForgotPasswordComponent,
    ]
})
export class ForgotPasswordModule {

} 