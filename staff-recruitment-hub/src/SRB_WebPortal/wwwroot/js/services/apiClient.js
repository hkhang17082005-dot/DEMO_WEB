const API_BASE_URL = 'http://localhost:8000/api';

const apiClient = {
   async request(endpoint, options = {}) {
      const url = `${API_BASE_URL}${endpoint}`;

      const headers = {
         'Content-Type': 'application/json',
         ...options.headers,
      };

      const config = { ...options, headers };
      const response = await fetch(url, config);

      if (!response.ok) {
         const error = await response.json();
         throw new Error(error.message || 'Có lỗi xảy ra');
      }

      return response.json();
   },
};

export default apiClient;
