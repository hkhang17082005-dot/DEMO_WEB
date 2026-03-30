import apiClient from './apiClient.js';

const postService = {
   savePost: (formData) => {
      return apiClient.post('/post/save-post', formData);
   },
};

export default postService;
