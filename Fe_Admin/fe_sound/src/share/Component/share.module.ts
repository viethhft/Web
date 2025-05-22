import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MusicWavesComponent } from './music-waves/music-waves.component';

@NgModule({
  declarations: [
    MusicWavesComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    MusicWavesComponent
  ]
})
export class ShareModule { } 