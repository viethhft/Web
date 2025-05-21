import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeManagementComponent } from './employee-management.component';

@NgModule({
    declarations: [
        EmployeeManagementComponent,
    ],
    imports: [
        CommonModule
    ],
    exports: [
        EmployeeManagementComponent,
    ]
})
export class EmployeeManagementModule { } 