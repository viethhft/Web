import { NgModule } from "@angular/core"
import { BrowserModule } from "@angular/platform-browser"
import { FormsModule } from "@angular/forms"
import { RouterModule } from "@angular/router"
import { HTTP_INTERCEPTORS } from "@angular/common/http"

import { AppRoutingModule } from "./app-routing.module"
import { AppComponent } from "./app.component"
import { LoginModule } from "./login/login.module"
import { ShareModule } from "../share/Component/share.module"
import { HttpClientModule } from "@angular/common/http"
import { AuthInterceptor } from "./interceptors/auth.interceptor";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'

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
        ShareModule,
        HttpClientModule
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        },
        provideAnimationsAsync()
    ],
    bootstrap: [AppComponent],
})
export class AppModule { }
