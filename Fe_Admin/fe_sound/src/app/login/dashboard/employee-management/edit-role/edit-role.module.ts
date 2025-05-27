import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EditRoleComponent } from './edit-role.component';
import { EditRoleRoutingModule } from './edit-role-routing.module';

@NgModule({
    declarations: [
        EditRoleComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        EditRoleRoutingModule,
    ],
    exports: [
        EditRoleComponent,
    ]
})
export class EditRoleModule { } 