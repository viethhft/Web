import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MusicManagementComponent } from './music-management.component';
import { AddMusicComponent } from './add-music/add-music.component';
import { ToastrModule } from 'ngx-toastr';
@NgModule({
    declarations: [
        MusicManagementComponent,
        AddMusicComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ToastrModule,
    ],
    exports: [
        MusicManagementComponent,
        AddMusicComponent
    ]
})
export class MusicManagementModule { } 