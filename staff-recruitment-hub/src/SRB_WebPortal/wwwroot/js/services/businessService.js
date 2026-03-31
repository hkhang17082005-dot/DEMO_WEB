import apiClient from './apiClient.js';

const businessService = {
   registerBusiness: (formData) => {
      return apiClient.post('/business/register', formData);
   },

   getBusinessJobPosts: (lastPostID, getSize) => {
      return apiClient.get(
         `/business/get-business-jp?LastPostID=${lastPostID || ''}&GetSize=${getSize || 10}`
      );
   },

   updateStatusApplyJob: (formData) => {
      return apiClient.post('/business/update-status-apply', formData);
   },
};

export default businessService;
