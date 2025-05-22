import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddMusicComponent } from './add-music.component';

const routes: Routes = [
    { path: '', component: AddMusicComponent },
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddMusicRoutingModule { } 