import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddEmployeeComponent } from './add-employee.component';
import { AddEmployeeRoutingModule } from './add-employee-routing.module';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
@NgModule({
    declarations: [
        AddEmployeeComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        AddEmployeeRoutingModule,
    ],
    exports: [
        AddEmployeeComponent,
    ]
})
export class AddEmlpoyeeModule { } 