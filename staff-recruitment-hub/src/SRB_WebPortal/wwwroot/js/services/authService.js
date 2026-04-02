import apiClient from './apiClient.js';

const authService = {
   login: (credentials) => {
      return apiClient.post('/auth/login', credentials);
   },

   register: (userData) => {
      return apiClient.post('/auth/register', userData);
   },

   logout: () => {
      return apiClient.delete('/auth/logout');
   },

   createProfile: (profileData) => {
      return apiClient.postForm('/auth/create-profile', profileData);
   },

   getUserInfo: () => {
      return apiClient.get('/auth/me');
   },
};

export default authService;
