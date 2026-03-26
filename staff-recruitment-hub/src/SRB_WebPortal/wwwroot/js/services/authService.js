import apiClient from './apiClient.js';

const authService = {
   login: (credentials) => {
      return apiClient.request('/auth/login', {
         method: 'POST',
         body: JSON.stringify(credentials),
      });
   },

   register: (userData) => {
      return apiClient.request('/auth/register', {
         method: 'POST',
         body: JSON.stringify(userData),
      });
   },

   createProfile: (profileData) => {
      return apiClient.request('/auth/create-profile', {
         method: 'POST',
         body: profileData,
      });
   },

   getUserInfo: () => {
      return apiClient.request('/auth/me');
   },
};

export default authService;
