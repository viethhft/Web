import { Component } from "@angular/core"
import { FormBuilder, FormGroup, Validators } from "@angular/forms"

@Component({
    selector: "app-forgot-password",
    templateUrl: './forgot-password.component.html',
    styleUrls: ["./forgot-password.component.scss"],
})
export class ForgotPasswordComponent {
    forgotPasswordForm: FormGroup
    isLoading = false
    emailSent = false

    constructor(private fb: FormBuilder) {
        this.forgotPasswordForm = this.fb.group({
            email: ["", [Validators.required, Validators.email]],
        })
    }

    onSubmit() {
        if (this.forgotPasswordForm.valid) {
            this.isLoading = true

            // Simulate email sending process
            setTimeout(() => {
                this.isLoading = false
                this.emailSent = true
            }, 1500)
        }
    }
}
