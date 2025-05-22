import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddMusicComponent } from './add-music.component';
import { FormsModule } from '@angular/forms';
import { AddMusicRoutingModule } from './add-music-routing.module';

@NgModule({
    declarations: [
        AddMusicComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        AddMusicRoutingModule
    ],
    exports: [
        AddMusicComponent
    ]
})
export class AddMusicModule { } 