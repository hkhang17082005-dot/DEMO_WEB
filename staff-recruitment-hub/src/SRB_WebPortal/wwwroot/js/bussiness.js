/**
 * Chức năng: Tải thông tin hồ sơ doanh nghiệp từ API
 * Phạm vi: Sử dụng cho các trang thuộc phân hệ Business
 */
async function loadBusinessProfile() {
    const companyElement = document.getElementById('companyName');

    try {
        // Gọi API Business đã định nghĩa
        const response = await fetch('/api/BusinessApi/profile');
        const result = await response.json();

        if (result.isSuccess) {
            // Cập nhật dữ liệu vào giao diện nếu tìm thấy thẻ ID
            if (companyElement) {
                companyElement.innerText = result.data.companyName;
                companyElement.classList.remove('animate-pulse', 'text-gray-400');
                companyElement.classList.add('text-blue-600');
            }
        } else if (result.reasonCode === "SIG_SESSION_TIMED_OUT") {
            // Tín hiệu hết hạn phiên làm việc từ BaseResponse
            window.location.href = '/Login';
        }
    } catch (error) {
        console.error("[BusinessJS] Lỗi tải dữ liệu:", error);
        if (companyElement) companyElement.innerText = "Lỗi tải thông tin";
    }
}

async function loadMyJobs() {
    const tableBody = document.getElementById('jobListTable');
    if (!tableBody) return;

    try {
        const response = await fetch('/api/BusinessApi/my-jobs');
        const result = await response.json();

        if (result.isSuccess && result.data.length > 0) {
            tableBody.innerHTML = result.data.map(job => `
                <tr class="hover:bg-gray-50 transition">
                    <td class="px-6 py-4 font-semibold text-gray-800">${job.title}</td>
                    <td class="px-6 py-4 text-gray-500">${new Date(job.createdAt).toLocaleDateString('vi-VN')}</td>
                    <td class="px-6 py-4">
                        <span class="px-3 py-1 rounded-full text-xs font-bold ${job.isActive ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}">
                            ${job.isActive ? 'Đang tuyển' : 'Đã đóng'}
                        </span>
                    </td>
                    <td class="px-6 py-4">
                        <button class="text-blue-600 hover:text-blue-800 font-medium">Sửa</button>
                    </td>
                </tr>
            `).join('');
        } else {
            tableBody.innerHTML = `<tr><td colspan="4" class="px-6 py-10 text-center text-gray-500">Bạn chưa có tin tuyển dụng nào.</td></tr>`;
        }
    } catch (error) {
        console.error("[MyJobs] Lỗi:", error);
        tableBody.innerHTML = `<tr><td colspan="4" class="px-6 py-10 text-center text-red-500">Không thể kết nối đến máy chủ.</td></tr>`;
    }
}

// Cập nhật lại DOMContentLoaded
document.addEventListener("DOMContentLoaded", () => {
    loadBusinessProfile(); // Tải tên công ty
    loadMyJobs();         // Tải danh sách việc làm
});

// Tự động kích hoạt khi DOM sẵn sàng
document.addEventListener("DOMContentLoaded", loadBusinessProfile);