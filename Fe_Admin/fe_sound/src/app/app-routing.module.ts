import { NgModule } from "@angular/core"
import { RouterModule, type Routes } from "@angular/router"
import { AuthGuard } from "./guards/auth.guard"
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component"

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
                loadChildren: () => import('./forgot-password/forgot-password.module').then(m => m.ForgotPasswordModule),
            }
        ]
    },
    {
        path: "unauthorized",
        component: UnauthorizedComponent
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
