import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';

import { EmployeeManagementComponent } from './employee-management.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { EditRoleComponent } from './edit-role/edit-role.component';

@NgModule({
    declarations: [
        EmployeeManagementComponent,
        AddEmployeeComponent,
        EditRoleComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ToastrModule
    ],
    exports: [
        EmployeeManagementComponent,
        AddEmployeeComponent,
        EditRoleComponent
    ]
})
export class EmployeeManagementModule { } 