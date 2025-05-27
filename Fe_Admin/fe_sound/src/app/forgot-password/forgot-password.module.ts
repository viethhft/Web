import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ForgotPasswordComponent } from './forgot-password.component';
import { ForgotPasswordRoutingModule } from './forgot-password-routing.module';
import { ShareModule } from '../../share/Component/share.module';

@NgModule({
    declarations: [
        ForgotPasswordComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        FormsModule,
        ForgotPasswordRoutingModule,
        ShareModule
    ],
    exports: [
        ForgotPasswordComponent,
    ]
})
export class ForgotPasswordModule {

} 