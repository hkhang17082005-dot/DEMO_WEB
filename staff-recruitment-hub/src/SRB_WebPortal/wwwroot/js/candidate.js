/**
 * Quản lý các tác vụ của Ứng viên (Upload CV, Tải lịch sử ứng tuyển)
 */
const CandidateModule = {
    // Tải CV lên Cloudinary
    async handleUploadCV(event) {
        const file = event.target.files[0];
        if (!file) return;

        const formData = new FormData();
        formData.append("FileCV", file);

        try {
            const response = await fetch('/api/Post/upload-my-cv', {
                method: 'POST',
                body: formData
            });
            const result = await response.json();

            if (result.isSuccess) {
                alert("Cập nhật CV thành công!");
                location.reload(); // Tải lại để cập nhật trạng thái
            } else {
                alert("Lỗi: " + result.message);
            }
        } catch (error) {
            console.error("Upload Error:", error);
        }
    },

    // Giả lập tải danh sách đã ứng tuyển (Bạn sẽ viết API này sau)
    async loadApplications() {
        const container = document.getElementById('applicationList');
        if (!container) return;

        // Logic fetch dữ liệu từ API nộp đơn (ví dụ /api/Candidate/applications)
        console.log("Đang tải danh sách ứng tuyển...");
    }
};

document.addEventListener("DOMContentLoaded", () => {
    CandidateModule.loadApplications();
});