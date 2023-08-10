import axios from 'axios';

const createFilesPath = async (file: File, userId: number) => {
  const formData = new FormData();
  formData.append('file', file);
  
  try {
    const response = await axios.post(`${process.env.REACT_APP_BACKEND_BASE_URL}/Reader/CreateFilesPath?userId=${userId}`, formData);
    return response.data;
  } catch (error) {
    console.error('Upload error:', error);
    throw new Error('Upload failed');
  }
};

const getUploadedFiles = async (userId: number) => {
    try {
      const response = await axios.get(`${process.env.REACT_APP_BACKEND_BASE_URL}/Reader/GetUploadedFiles`, {
        params: { userID: userId },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching uploaded files:', error);
      throw new Error('Fetch failed');
    }
  };

const readerService = {
  createFilesPath,
  getUploadedFiles
};

export default readerService;
