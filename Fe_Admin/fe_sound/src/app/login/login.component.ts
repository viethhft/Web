import { Component } from "@angular/core"
import { FormBuilder, FormGroup, Validators } from "@angular/forms"

@Component({
    selector: "app-login",
    templateUrl: './login.component.html',
    styleUrls: ["./login.component.scss"],
})
export class LoginComponent {
    loginForm: FormGroup
    isLoading = false
    showPassword = false

    constructor(private fb: FormBuilder) {
        this.loginForm = this.fb.group({
            email: ["", [Validators.required, Validators.email]],
            password: ["", Validators.required],
        })
    }

    onSubmit() {
        if (this.loginForm.valid) {
            this.isLoading = true

            // Simulate login process
            setTimeout(() => {
                this.isLoading = false
            }, 1500)
        }
    }

    togglePasswordVisibility() {
        this.showPassword = !this.showPassword
    }
}
