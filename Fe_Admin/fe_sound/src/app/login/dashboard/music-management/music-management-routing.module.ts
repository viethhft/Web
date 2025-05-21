import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MusicManagementComponent } from './music-management.component';

const routes: Routes = [
    { path: 'music-management', component: MusicManagementComponent },
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MusicManagementRoutingModule { } 