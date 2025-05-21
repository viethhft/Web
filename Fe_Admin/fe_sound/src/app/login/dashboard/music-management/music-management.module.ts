import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MusicManagementComponent } from './music-management.component';


@NgModule({
    declarations: [
        MusicManagementComponent
    ],
    imports: [
        CommonModule
    ],
    exports: [
        MusicManagementComponent
    ]
})
export class MusicManagementModule { } 