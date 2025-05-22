import { Component } from "@angular/core"
import { NgForm } from "@angular/forms"
import { BaseModel } from "../../../../../share/Dtos/Base.model"

interface MusicForm {
    title: string;
    icon: File | null;
    music: File | null;
}

@Component({
    selector: "app-add-music",
    templateUrl: "./add-music.component.html",
    styleUrls: ["./add-music.component.scss"],
})
export class AddMusicComponent extends BaseModel {
    isSubmitting = false
    selectedFile: File | null = null
    previewUrl: string | null = null
    iconFile: File | null = null
    musicFile: File | null = null
    musicPreviewUrl: string | null = null
    iconPreviewUrl: string | null = null

    categories = ["Thư giãn", "Ru ngủ", "Thiên nhiên"]

    music: MusicForm = {
        title: '',
        icon: null,
        music: null
    }

    constructor() {
        super();
    }

    onFileSelected(event: Event) {
        const input = event.target as HTMLInputElement
        if (input.files && input.files.length) {
            this.selectedFile = input.files[0]

            // Create preview URL for audio file
            if (this.selectedFile.type.startsWith("audio/")) {
                this.previewUrl = URL.createObjectURL(this.selectedFile)
            } else {
                this.previewUrl = null
            }
        }
    }

    onIconFileSelected(event: Event) {
        const input = event.target as HTMLInputElement
        if (input.files && input.files.length) {
            this.iconFile = input.files[0]
            this.music.icon = this.iconFile

            // Create preview URL for image file
            if (this.iconFile.type.startsWith("image/")) {
                this.iconPreviewUrl = URL.createObjectURL(this.iconFile)
            } else {
                this.iconPreviewUrl = null
            }
        }
    }

    onMusicFileSelected(event: Event) {
        const input = event.target as HTMLInputElement
        if (input.files && input.files.length) {
            this.musicFile = input.files[0]
            this.music.music = this.musicFile

            // Create preview URL for audio file
            if (this.musicFile.type.startsWith("audio/")) {
                this.musicPreviewUrl = URL.createObjectURL(this.musicFile)
            } else {
                this.musicPreviewUrl = null
            }
        }
    }

    onSubmit(form: NgForm) {
        if (form.valid && this.musicFile) {
            this.isSubmitting = true
            const formData = new FormData()
            formData.append("title", this.music.title)

            if (this.iconFile) {
                formData.append("icon", this.iconFile)
            }
            formData.append("music", this.musicFile)

            // Call your service to upload the file here
            // Example: this.soundService.uploadMusic(formData).subscribe(...)
        }
    }

    closeModal() {
    }

    formatFileSize(bytes: number): string {
        if (bytes === 0) return "0 Bytes"
        const k = 1024
        const sizes = ["Bytes", "KB", "MB", "GB"]
        const i = Math.floor(Math.log(bytes) / Math.log(k))
        return Number.parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i]
    }
}
