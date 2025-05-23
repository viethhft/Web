import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from "@angular/core"
import { SoundService } from "../../../../services/sound/sound.service"
import { ConvertDate } from "../../../../share/Services/Extentions"
import { GetList } from "../../../../share/Dtos/Dtos.Share"
import { AdminSound } from "../../../../services/sound/sound.dtos"
import { BaseModel, DataSettingForm } from "../../../../share/Dtos/Base.model"
import { AddMusicComponent } from "./add-music/add-music.component"
import { MatDialog } from "@angular/material/dialog"
import { ToastrService } from "ngx-toastr"
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
        PageSize: 5,
        PageNumber: 1,
    }
    musicFiles: AdminSound[] = [];
    audio: HTMLAudioElement | null = null;
    currentFileChoose: number = -1;
    currentTime = 0;
    duration = 0;
    urlAudioPlaying?: string = "";
    nameAudioPlaying?: string = "";
    hideAudio: boolean = false;
    convertDate = ConvertDate;
    categories = ["Tất cả", "Đang hoạt động", "Ngừng hoạt động"]
    sortOptions = ["Sắp xếp theo", "Ngày thêm", "Tên"]
    constructor(private soundService: SoundService, private cd: ChangeDetectorRef, private mat: MatDialog, private logService: ToastrService) {
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
                        file: this.changeDataToFile(item.content, item.contentType, item.fileName),
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

    changeDataToFile(data: string, type: string, name: string): File {
        const byteString = atob(data);
        const arrayBuffer = new ArrayBuffer(byteString.length);
        const intArray = new Uint8Array(arrayBuffer);

        for (let i = 0; i < byteString.length; i++) {
            intArray[i] = byteString.charCodeAt(i);
        }

        return new File([intArray], name, { type: type });
    }

    onSearch(event: Event): void {
        this.searchQuery = (event.target as HTMLInputElement).value
        // Implement search logic here
    }

    onCategoryChange(event: Event): void {
        this.selectedCategory = (event.target as HTMLSelectElement).value
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
        this.urlAudioPlaying = this.musicFiles.find(c => c.id === file.id)?.image;
        this.nameAudioPlaying = this.musicFiles.find(c => c.id === file.id)?.name;
        if (this.audio && file.id === this.currentFileChoose) {
            if (!this.audio.paused)
                this.audio.pause();
            else {
                this.hideAudio = false;
                this.audio.play();
            }
            this.cd.detectChanges();
            return;

        }
        else if (this.audio && file.id !== this.currentFileChoose) {
            this.audio.pause();
            this.hideAudio = false;
            this.audio = new Audio(URL.createObjectURL(file.file));
            this.audio.onended = () => {
                this.audio = null;
            };
            this.audio.addEventListener('timeupdate', () => {
                this.currentTime = this.audio!.currentTime;
            });

            this.audio.addEventListener('loadedmetadata', () => {
                this.duration = this.audio!.duration;
            });

            this.audio.addEventListener('ended', () => {
                this.currentTime = 0;
            });
        }
        else {
            this.audio = new Audio(URL.createObjectURL(file.file));
            this.audio.onended = () => {
                this.audio = null;
            };
            this.audio.addEventListener('timeupdate', () => {
                this.currentTime = this.audio!.currentTime;
            });

            this.audio.addEventListener('loadedmetadata', () => {
                this.duration = this.audio!.duration;
            });

            this.audio.addEventListener('ended', () => {
                this.currentTime = 0;
            });
        }
        if (this.currentFileChoose === file.id) {
            this.audio.pause();
            this.currentFileChoose = -1;
        } else {
            this.audio.play().catch(err => console.error(err));
            this.currentFileChoose = file.id;
        }
        this.cd.detectChanges();
    }

    formatTime(seconds: number): string {
        const mins = Math.floor(seconds / 60);
        const secs = Math.floor(seconds % 60);
        return `${this.pad(mins)}:${this.pad(secs)}`;
    }

    pad(num: number): string {
        return num < 10 ? '0' + num : '' + num;
    }

    seek(event: MouseEvent) {
        const container = event.currentTarget as HTMLElement;
        const rect = container.getBoundingClientRect();
        const clickX = event.clientX - rect.left;
        const ratio = clickX / rect.width;
        this.audio!.currentTime = ratio * this.duration;
    }

    hideFormAudio() {
        this.hideAudio = !this.hideAudio;
        this.cd.detectChanges();
    }

    continuteAction() {
        if (this.audio?.paused) {
            this.audio.play();
        }
        else {
            this.audio!.pause();
        }
    }
    editMusic(file: AdminSound): void {
        const data: DataSettingForm = {
            width: "600px",
            height: "400px",
            data: {
                file: file,
                title: "Sửa âm thanh",
                status: false
            },
        }
        this.showDialog(AddMusicComponent, data).afterClosed().subscribe((result) => {
            if (result) {
                if (result.load) {
                    this.musicFiles = [];
                    this.getDataSound(this.dataGet);
                }
            } else {
                console.error("Lỗi khi thêm âm thanh mới")
            }
        })
    }

    deleteMusic(file: AdminSound): void {
        this.soundService.deleteSound(file.id).subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.getDataSound(this.dataGet);
                    this.cd.detectChanges();
                    this.logService.success(response.message, "Thông báo");
                    console.log(response.message)
                } else {
                    this.logService.error(response.message, "Thông báo");
                }
            },
            (error) => {
                console.error("Lỗi khi gọi API", error);
                this.logService.success("Có lỗi xảy ra vui lòng thử lại sau hoặc liên hệ nhà phát triển", "Thông báo");
            }
        )
    }

    activeMusic(file: AdminSound): void {
        this.soundService.activateSound(file.id).subscribe(
            (response) => {
                if (response.isSuccess) {
                    this.getDataSound(this.dataGet);
                    this.cd.detectChanges();
                    this.logService.success(response.message, "Thông báo");
                    console.log(response.message)
                } else {
                    this.logService.error(response.message, "Thông báo");
                }
            },
            (error) => {
                console.error("Lỗi khi gọi API", error);
                this.logService.error("Có lỗi xảy ra vui lòng thử lại sau hoặc liên hệ nhà phát triển", "Thông báo");
            }
        )
    }

    uploadNewFile(): void {
        const data: DataSettingForm = {
            width: "600px",
            height: "400px",
            data: {
                title: "Thêm âm thanh mới",
                status: true,
            },
        }
        this.showDialog(AddMusicComponent, data).afterClosed().subscribe((result) => {
            if (result) {
                if (result.load) {
                    this.musicFiles = [];
                    this.getDataSound(this.dataGet);
                }
            } else {
                console.error("Lỗi khi thêm âm thanh mới")
            }
        })
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
