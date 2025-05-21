import { Component } from "@angular/core"
import { FormBuilder, FormGroup } from "@angular/forms"

@Component({
    selector: "app-settings",
    templateUrl: './settings.component.html',
    styleUrls: ["./settings.component.scss"],
})
export class SettingsComponent {
    accountForm: FormGroup
    notificationSettings = {
        emailNotifications: true,
        systemNotifications: true,
        updateNotifications: false,
    }
    systemSettings = {
        darkMode: true,
    }

    constructor(private fb: FormBuilder) {
        this.accountForm = this.fb.group({
            username: ["Admin"],
            email: ["admin@soundsleep.com"],
            newPassword: [""],
            confirmPassword: [""],
        })
    }

    saveAccountChanges(): void {
        console.log("Saving account changes:", this.accountForm.value)
        // Implement save logic here
    }

    toggleNotification(setting: keyof typeof this.notificationSettings): void {
        this.notificationSettings[setting] = !this.notificationSettings[setting]
    }

    toggleDarkMode(): void {
        this.systemSettings.darkMode = !this.systemSettings.darkMode
    }

    saveSystemChanges(): void {
        console.log("Saving system changes")
        // Implement save logic here
    }
}
