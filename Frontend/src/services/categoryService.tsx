import axios from 'axios';

const getUserCategories = async(userId: number) => {
    try{
        const response = await axios.get(`${process.env.REACT_APP_BACKEND_BASE_URL}/Category/GetUserCategories/${userId}`);
          console.log(response.data);
          return response.data;          
        }
    catch(error){
        console.error('Error fetching categories data:', error);
        throw new Error('Fetch failed');      
    }
};


const categoryService = {
    getUserCategories
  };
  
  export default categoryService;
  