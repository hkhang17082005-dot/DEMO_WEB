const API_BASE_URL = 'http://localhost:8000/api';

const apiClient = {
   async baseRequest(endpoint, config = {}) {
      const url = `${API_BASE_URL}${endpoint}`;

      config.credentials = 'include';

      const response = await fetch(url, config);

      if (!response.ok) {
         const error = await response.json().catch(() => ({}));

         throw new Error(error.message || error.title || `Lỗi hệ thống (${response.status})`);
      }

      return response.json();
   },

   async post(endpoint, data = {}) {
      return this.baseRequest(endpoint, {
         method: 'POST',
         headers: {
            'Content-Type': 'application/json',
         },
         body: JSON.stringify(data),
      });
   },

   // Chuyên gửi Form/File
   async postForm(endpoint, formData) {
      return this.baseRequest(endpoint, {
         method: 'POST',
         body: formData,
      });
   },

   // Chuyên lấy dữ liệu [Get]
   async get(endpoint) {
      return this.baseRequest(endpoint, {
         method: 'GET',
      });
   },

   async delete(endpoint, data = null) {
      const config = {
         method: 'DELETE',
      };

      if (data) {
         config.headers = {
            'Content-Type': 'application/json',
         };
         config.body = JSON.stringify(data);
      }

      return this.baseRequest(endpoint, config);
   },
};

export default apiClient;
