import { Component } from "@angular/core"

interface MusicFile {
    id: number
    title: string
    artist: string
    duration: string
    category: string
    plays: number
    dateAdded: string
}

@Component({
    selector: "app-music-management",
    templateUrl: "./music-management.component.html",
    styleUrls: ["./music-management.component.scss"],
})
export class MusicManagementComponent {
    searchQuery = ""
    selectedCategory = "Tất cả thể loại"
    sortOption = "Sắp xếp theo"
    currentPage = 1
    totalPages = 1

    musicFiles: MusicFile[] = [
        {
            id: 1,
            title: "Bản nhạc thư giãn #1",
            artist: "Nghệ sĩ A",
            duration: "3:45",
            category: "Thư giãn",
            plays: 120,
            dateAdded: "15/05/2025",
        },
        {
            id: 2,
            title: "Bản nhạc thư giãn #2",
            artist: "Nghệ sĩ B",
            duration: "4:20",
            category: "Thư giãn",
            plays: 95,
            dateAdded: "14/05/2025",
        },
        {
            id: 3,
            title: "Bản nhạc ru ngủ #1",
            artist: "Nghệ sĩ C",
            duration: "5:10",
            category: "Ru ngủ",
            plays: 210,
            dateAdded: "12/05/2025",
        },
        {
            id: 4,
            title: "Âm thanh thiên nhiên #1",
            artist: "Nghệ sĩ A",
            duration: "6:30",
            category: "Thiên nhiên",
            plays: 180,
            dateAdded: "10/05/2025",
        },
        {
            id: 5,
            title: "Âm thanh thiên nhiên #2",
            artist: "Nghệ sĩ D",
            duration: "4:55",
            category: "Thiên nhiên",
            plays: 150,
            dateAdded: "08/05/2025",
        },
        {
            id: 6,
            title: "Bản nhạc ru ngủ #2",
            artist: "Nghệ sĩ B",
            duration: "5:45",
            category: "Ru ngủ",
            plays: 135,
            dateAdded: "05/05/2025",
        },
        {
            id: 7,
            title: "Bản nhạc thư giãn #3",
            artist: "Nghệ sĩ C",
            duration: "4:15",
            category: "Thư giãn",
            plays: 90,
            dateAdded: "01/05/2025",
        },
    ]

    categories = ["Tất cả thể loại", "Thư giãn", "Ru ngủ", "Thiên nhiên"]
    sortOptions = ["Sắp xếp theo", "Lượt phát", "Ngày thêm", "Tên"]

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
        if (this.currentPage > 1) {
            this.currentPage--
        }
    }

    nextPage(): void {
        if (this.currentPage < this.totalPages) {
            this.currentPage++
        }
    }

    playMusic(file: MusicFile): void {
        console.log(`Playing: ${file.title}`)
        // Implement play logic here
    }

    editMusic(file: MusicFile): void {
        console.log(`Editing: ${file.title}`)
        // Implement edit logic here
    }

    deleteMusic(file: MusicFile): void {
        console.log(`Deleting: ${file.title}`)
        // Implement delete logic here
    }

    uploadNewFile(): void {
        console.log("Uploading new file")
        // Implement upload logic here
    }
}
