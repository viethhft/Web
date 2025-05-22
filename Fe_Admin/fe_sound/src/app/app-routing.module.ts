import { NgModule } from "@angular/core"
import { RouterModule, type Routes } from "@angular/router"
import { AuthGuard } from "./guards/auth.guard"

const routes: Routes = [
    {
        path: "admin",
        children: [
            {
                path: "login",
                loadChildren: () => import('./login/login.module').then(m => m.LoginModule),
            },
            {
                path: "dashboard",
                canActivate: [AuthGuard],
                loadChildren: () => import('./login/dashboard/dashboard.module').then(m => m.DashboardModule),
            },
            {
                path: "forgot-password",
                loadChildren: () => import('./login/login.module').then(m => m.LoginModule),
            }
        ]
    },
    {
        path: "",
        redirectTo: "/admin/login",
        pathMatch: "full"
    },
    {
        path: "**",
        redirectTo: "/admin/login"
    }
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }
