import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { OverviewRoutingModule } from './overview-routing.module';
import { OverviewComponent } from './overview.component';

@NgModule({
    declarations: [
        OverviewComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        OverviewRoutingModule
    ],
    exports: [
        OverviewComponent,
    ]
})
export class OverviewModule {

} 