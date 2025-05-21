import { NgModule } from "@angular/core"
import { BrowserModule } from "@angular/platform-browser"
import { FormsModule } from "@angular/forms"
import { RouterModule } from "@angular/router"

import { AppRoutingModule } from "./app-routing.module"
import { AppComponent } from "./app.component"
import { LoginModule } from "./login/login.module"
import { ShareModule } from "./share/Component/share.module"

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        RouterModule,
        LoginModule,
        ShareModule
    ],
    providers: [],
    bootstrap: [AppComponent],
})
export class AppModule { }
