const API_BASE_URL = 'http://localhost:8000/api';

const apiClient = {
   async request(endpoint, options = {}) {
      const url = `${API_BASE_URL}${endpoint}`;

      const headers = { ...options.headers };

      if (options.body instanceof FormData) {
         delete headers['Content-Type'];
         delete headers['content-type'];
      } else if (options.body && typeof options.body === 'object') {
         headers['Content-Type'] = 'application/json';
      }

      const config = {
         ...options,
         headers,
         credentials: 'include',
      };

      const response = await fetch(url, config);

      if (!response.ok) {
         const error = await response.json();
         throw new Error(error.message || error.title || 'Có lỗi xảy ra');
      }

      return response.json();
   },
};

export default apiClient;
