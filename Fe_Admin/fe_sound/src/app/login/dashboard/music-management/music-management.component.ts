import { ChangeDetectorRef, Component, OnInit } from "@angular/core"
import { SoundService } from "../../../../services/sound/sound.service"
import { ConvertDate } from "../../../../share/Services/Extentions"
import { GetList } from "../../../../share/Dtos/Dtos.Share"
import { AdminSound } from "../../../../services/sound/sound.dtos"
import { BaseModel, DataSettingForm } from "../../../../share/Dtos/Base.model"
import { AddMusicComponent } from "./add-music/add-music.component"
import { MatDialog } from "@angular/material/dialog"

@Component({
    selector: "app-music-management",
    templateUrl: "./music-management.component.html",
    styleUrls: ["./music-management.component.scss"],
})
export class MusicManagementComponent extends BaseModel implements OnInit {
    searchQuery = ""
    selectedCategory = "Tất cả thể loại"
    sortOption = "Sắp xếp theo"
    dataGet: GetList = {
        PageSize: 6,
        PageNumber: 1,
    }
    musicFiles: AdminSound[] = []
    convertDate = ConvertDate;
    categories = ["Tất cả thể loại", "Thư giãn", "Ru ngủ", "Thiên nhiên"]
    sortOptions = ["Sắp xếp theo", "Lượt phát", "Ngày thêm", "Tên"]
    constructor(private soundService: SoundService, private cd: ChangeDetectorRef, private mat: MatDialog) {
        super(mat);
        this.soundService = soundService
    }

    ngOnInit(): void {
        this.getDataSound(this.dataGet);
    }

    getDataSound(dataGet: GetList) {
        this.IsLoading = true;
        this.soundService.getSoundByAdmin(dataGet).subscribe(
            (response) => {
                if (response.isSuccess) {
                    console.log("Lấy danh sách âm thanh thành công", response.data)
                    this.musicFiles = response.data.data.map((item) => ({
                        ...item,
                        file: this.changeFile(item.content, item.contentType, item.name),
                    }));
                    this.TotalPage = response.data.totalPage;
                    this.CurrentPage = response.data.currentPage;
                    this.IsLoading = false;
                    this.cd.detectChanges();
                } else {
                    this.IsLoading = false;
                    console.error("Lỗi khi lấy danh sách âm thanh", response.message)
                }
            },
            (error) => {
                this.IsLoading = false;
                console.error("Lỗi khi gọi API", error)
            }
        )
    }

    getCategoryClass(category: string): string {
        switch (category) {
            case "Thư giãn":
                return "category-relaxation"
            case "Ru ngủ":
                return "category-sleep"
            case "Thiên nhiên":
                return "category-nature"
            default:
                return ""
        }
    }

    changeFile(data: any, type: string, name: string): File {
        const byteArray = new Uint8Array(data);

        const blob = new Blob([byteArray], { type: type });

        const file = new File([blob], name, { type: type });

        return file;
    }

    onSearch(event: Event): void {
        this.searchQuery = (event.target as HTMLInputElement).value
        // Implement search logic here
    }

    onCategoryChange(event: Event): void {
        this.selectedCategory = (event.target as HTMLSelectElement).value
        // Implement category filter logic here
    }

    onSortChange(event: Event): void {
        this.sortOption = (event.target as HTMLSelectElement).value
        // Implement sort logic here
    }

    previousPage(): void {
        if (this.CurrentPage > 1) {
            this.CurrentPage--
        }
    }

    nextPage(): void {
        if (this.CurrentPage < this.TotalPage) {
            this.CurrentPage++
        }
    }

    playMusic(file: AdminSound): void {
        console.log(`Playing: ${file.name}`)
        // Implement play logic here
    }

    editMusic(file: AdminSound): void {
        console.log(`Editing: ${file.name}`)
        // Implement edit logic here
    }

    deleteMusic(file: AdminSound): void {
        console.log(`Deleting: ${file.name}`)
        // Implement delete logic here
    }

    uploadNewFile(): void {
        const data: DataSettingForm = {
            title: "Thêm âm thanh mới",
            width: "600px",
            height: "400px",
            data: {
                title: "Thêm âm thanh mới",
                width: "600px",
                height: "400px",
            },
        }
        this.showDialog(AddMusicComponent, data).afterClosed().subscribe((result) => {
            if (result) {
                console.log("Thêm âm thanh mới thành công", result)
                this.getDataSound(this.dataGet)
            } else {
                console.error("Lỗi khi thêm âm thanh mới")
            }
        })
    }

    getTimeFile(file: File): Promise<string> {
        return new Promise((resolve, reject) => {
            const audio = document.createElement('audio');
            audio.preload = 'metadata';
            const url = URL.createObjectURL(file);
            audio.src = url;

            audio.onloadedmetadata = () => {
                URL.revokeObjectURL(url); // dọn bộ nhớ

                const duration = audio.duration;
                const minutes = Math.floor(duration / 60);
                const seconds = Math.round(duration % 60).toString().padStart(2, '0');
                resolve(`${minutes}:${seconds}`);
            };

            audio.onerror = () => {
                reject('Không thể đọc metadata của file MP3');
            };
        });
    }
    goToPage(page: number) {
        if (page >= 1 && page <= this.TotalPage && page !== this.CurrentPage) {
            this.IsLoading = true
            this.CurrentPage = page
            this.dataGet.PageNumber = page
            this.getDataSound(this.dataGet)
        }
    }
}
