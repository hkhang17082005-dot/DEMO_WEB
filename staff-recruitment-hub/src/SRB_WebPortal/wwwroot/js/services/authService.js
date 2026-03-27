import apiClient from './apiClient.js';

const authService = {
   login: (credentials) => {
      return apiClient.post('/auth/login', credentials);
   },

   register: (userData) => {
      return apiClient.post('/auth/register', userData);
   },

   createProfile: (profileData) => {
      // Nếu profileData là Object JSON
      return apiClient.post('/auth/create-profile', profileData);

      // FormData chứa file/ảnh
      // return apiClient.postForm('/auth/create-profile', profileData);
   },

   getUserInfo: () => {
      return apiClient.get('/auth/me');
   },
};

export default authService;
