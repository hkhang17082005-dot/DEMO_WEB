import apiClient from './apiClient.js';

const candidateService = {
    /**
     * Lấy danh sách công việc đã ứng tuyển
     * @param {string} lastId ID của hồ sơ cuối cùng (dùng để phân trang/load more)
     * @param {number} size Số lượng lấy mỗi lần
     * @param {string|number} status Lọc theo trạng thái (để trống để lấy tất cả)
     */
    getMyApplications: async (lastId = "", size = 5, status = "") => {
        let url = `/CandidateApi/my-applications?size=${size}`;
        if (lastId) url += `&lastId=${lastId}`;
        if (status !== "") url += `&status=${status}`;

        // Gọi qua apiClient có sẵn trong dự án của bạn (thường đã config sẵn xử lý token, lỗi...)
        return await apiClient.get(url);
    }
};

export default candidateService;