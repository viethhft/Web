import { ChangeDetectorRef, Component, Inject, OnInit } from "@angular/core"
import { NgForm } from "@angular/forms"
import { BaseModel } from "../../../../../share/Dtos/Base.model"
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from "@angular/material/dialog";
import { SoundService } from "../../../../../services/sound/sound.service";
import { AddSound } from "../../../../../services/sound/sound.dtos";
@Component({
    selector: "app-add-music",
    templateUrl: "./add-music.component.html",
    styleUrls: ["./add-music.component.scss"],
})
export class AddMusicComponent extends BaseModel implements OnInit {
    iconFile: File | null = null
    musicFile: File | null = null
    musicPreviewUrl: string | null = null
    iconPreviewUrl: string | null = null
    categories = ["Thư giãn", "Ru ngủ", "Thiên nhiên"]

    music: AddSound = {
        name: "",
        image: undefined,
        file: undefined,
        token: "",
    }

    constructor(private dialogRef: MatDialogRef<AddMusicComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private soundService: SoundService, private cd: ChangeDetectorRef) {
        super();
    }

    ngOnInit(): void {
        if (this.data.file) {
            const fileData = this.data.file;
            this.iconPreviewUrl = fileData.image;
            const iconBlob = new Blob([fileData.audioData], { type: 'image/png' }); // hoặc 'audio/mp3'
            this.iconFile = new File([iconBlob], fileData.audioName, { type: 'image/png' });

            this.musicFile = fileData.file;
            this.musicPreviewUrl = URL.createObjectURL(fileData.file);
            this.cd.detectChanges();
        }
    }

    onIconFileSelected(event: Event) {
        const input = event.target as HTMLInputElement
        if (input.files && input.files.length) {
            this.iconFile = input.files[0]
            this.music.image = this.iconFile

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
            this.music.file = this.musicFile

            // Create preview URL for audio file
            if (this.musicFile.type.startsWith("audio/")) {
                this.musicPreviewUrl = URL.createObjectURL(this.musicFile)
            } else {
                this.musicPreviewUrl = null
            }
        }
    }

    onSubmit(form: NgForm) {
        form.form.markAllAsTouched();
        if (form.valid && this.musicFile) {
            this.IsLoading = true;
            this.music.token = localStorage.getItem(this.TOKEN_KEY) || "";

            this.soundService.addSound(this.music).subscribe(
                (response) => {
                    if (response.isSuccess) {

                        this.closeModal("Thêm âm thanh thành công", true);
                    } else {
                        console.error("Lỗi khi thêm âm thanh", response.message)
                        this.IsLoading = false
                    }
                },
                (error) => {
                    console.error("Lỗi khi gọi API", error)
                    this.IsLoading = false
                }
            );
        }
    }

    closeModal(message: string, load: boolean = false) {
        this.dialogRef.close({ message: message, load: load });
    }

    formatFileSize(bytes: number): string {
        if (bytes === 0) return "0 Bytes"
        const k = 1024
        const sizes = ["Bytes", "KB", "MB", "GB"]
        const i = Math.floor(Math.log(bytes) / Math.log(k))
        return Number.parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i]
    }
}
