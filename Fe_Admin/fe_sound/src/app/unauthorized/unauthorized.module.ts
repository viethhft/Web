import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FormsModule } from "@angular/forms"
import { ShareModule } from '../../share/Component/share.module';
import { UnauthorizedComponent } from './unauthorized.component';
import { UnauthorizedRoutingModule } from './unauthorized-routing.module';

@NgModule({
  declarations: [
    UnauthorizedComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    FormsModule,
    ShareModule,
    UnauthorizedRoutingModule
  ],
  exports: [
    UnauthorizedComponent,
  ]
})
export class LoginModule { } 