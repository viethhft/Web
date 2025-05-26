import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';

import { EmployeeManagementComponent } from './employee-management.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';

@NgModule({
    declarations: [
        EmployeeManagementComponent,
        AddEmployeeComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ToastrModule
    ],
    exports: [
        EmployeeManagementComponent,
        AddEmployeeComponent
    ]
})
export class EmployeeManagementModule { } 