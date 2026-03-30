import apiClient from './apiClient.js';

const postService = {
   savePost: (formData) => {
      return apiClient.post('/post/save-post', formData);
   },

   loadHomeJobPosts: (lastId, pageSize) => {
      return apiClient.get(`/post/load-list?LastPostID=${lastId}&PageSize=${pageSize}`);
   },

   applyJobPost: (formData) => {
      return apiClient.postForm('/post/apply-job-post', formData);
   },
};

export default postService;
