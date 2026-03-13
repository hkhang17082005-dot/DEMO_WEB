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

   getUserInfo: () => {
      return apiClient.request('/auth/me');
   },
};

export default authService;
