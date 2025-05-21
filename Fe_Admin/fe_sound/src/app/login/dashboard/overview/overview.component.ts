import { Component } from "@angular/core"

@Component({
    selector: "app-overview",
    templateUrl: "./overview.component.html",
    styleUrls: ["./overview.component.scss"],
})
export class OverviewComponent {
    // Stats data
    stats = [
        { title: "Tổng nhân viên", value: 24, change: 12, icon: "users" },
        { title: "Tổng file âm nhạc", value: 156, change: 8, icon: "file-music" },
        { title: "Lượt phát", value: 2845, change: 23, icon: "bar-chart" },
        { title: "Người dùng hoạt động", value: 18, change: 5, icon: "user" },
    ]

    // Chart data
    categoryPlays = [
        { category: "Thư giãn", plays: 305, height: 120, color: "blue" },
        { category: "Ru ngủ", plays: 345, height: 180, color: "purple" },
        { category: "Thiên nhiên", plays: 330, height: 150, color: "green" },
    ]

    weeklyPlays = [40, 60, 30, 70, 50, 80, 65]

    // Recent activities
    recentActivities = [
        { user: "Nguyễn Văn A", action: "đã tải lên file âm nhạc mới", time: "2 giờ trước" },
        { user: "Trần Thị B", action: "đã chỉnh sửa thông tin cá nhân", time: "3 giờ trước" },
        { user: "Lê Văn C", action: "đã thêm bản nhạc vào danh sách phát", time: "5 giờ trước" },
        { user: "Phạm Thị D", action: "đã đăng nhập vào hệ thống", time: "6 giờ trước" },
        { user: "Hoàng Văn E", action: "đã cập nhật bản nhạc", time: "8 giờ trước" },
    ]

    // Popular music files
    popularMusicFiles = [
        { title: "Bản nhạc ru ngủ #1", plays: 210, category: "Ru ngủ" },
        { title: "Âm thanh thiên nhiên #1", plays: 180, category: "Thiên nhiên" },
        { title: "Âm thanh thiên nhiên #2", plays: 150, category: "Thiên nhiên" },
        { title: "Bản nhạc ru ngủ #2", plays: 135, category: "Ru ngủ" },
        { title: "Bản nhạc thư giãn #1", plays: 120, category: "Thư giãn" },
    ]

    // Top 5 music files
    topMusicFiles = [
        { title: "Bản nhạc ru ngủ #1", plays: 210, percent: 100 },
        { title: "Âm thanh thiên nhiên #1", plays: 180, percent: 85 },
        { title: "Âm thanh thiên nhiên #2", plays: 150, percent: 71 },
        { title: "Bản nhạc ru ngủ #2", plays: 135, percent: 64 },
        { title: "Bản nhạc thư giãn #1", plays: 120, percent: 57 },
    ]

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
}
