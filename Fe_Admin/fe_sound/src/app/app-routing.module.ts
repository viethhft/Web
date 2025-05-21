import { NgModule } from "@angular/core"
import { RouterModule, type Routes } from "@angular/router"

const routes: Routes = [
    {
        path: "admin",
        loadChildren: () => import('./login/login.module').then(m => m.LoginModule),
    },
    { path: "", redirectTo: "/admin/login", pathMatch: "full" }
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }
