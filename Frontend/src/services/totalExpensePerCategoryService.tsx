import axios from 'axios';

const getCategoriesSumPerTimePeriod = async(month : number, year: number , userId: number) => {
    const url = `${process.env.REACT_APP_BACKEND_BASE_URL}/TotalExpensePerCategory/GetCategoriesSumPerTimePeriod/${month}/${year}/?userID=${userId}`;
    console.log(`the url is ${url}`);        
    try{
        const response = await axios.get(url);
        console.log(response.data);
        return response.data;            
        }
    catch(error){
        console.log(`error fetching categories sum on time period with error: ${error}`);
        throw new Error(`error fetching categories sum on time period with error `)
    }
}

const getCategorySumPerTimePeriod = async(category:string, month:number, year: number, userId: number) => {
    const url =  `${process.env.REACT_APP_BACKEND_BASE_URL}/TotalExpensePerCategory/GetTotalExpensesSumPerMonth?category=${category}&month=${month}&year=${year}&userID=${userId}`;
    try {
        const response = await axios.get(url);
        return response.data;
      } catch (error) {
        console.log(`error fetching categories sum on time period with error: ${error}`);
        throw new Error(`error fetching categories sum on time period with error `)
      }
}

const CreateCategoriesSum = async(fromDate:string, userId: number) => {
    const url =  `${process.env.REACT_APP_BACKEND_BASE_URL}/TotalExpensePerCategory/CreateTotalExpensesPerCategories?fromDate=${fromDate}&&userID=${userId}`;
    try {
        const response = await axios.post(url);
        return response.data;
      } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error(`fetch has failed `)
      }
}

const totalExpensePerCategoryService = {
    getCategoriesSumPerTimePeriod,
    getCategorySumPerTimePeriod,
    CreateCategoriesSum
}

export default totalExpensePerCategoryService;