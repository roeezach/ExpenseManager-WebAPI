import axios from 'axios';
import { fileTypeParser } from '../utils/FileTypes';
import Cookies from 'universal-cookie';

const cookies = new Cookies();

const createMappedExpenses = async (fileName: string, fileType:string, chargedDate: string, userId:number) => {
    const parsedFileType = fileTypeParser(fileType);
    console.log(`parsed file type was ${parsedFileType}`);       
    if(parsedFileType !== 0 && parsedFileType !== 1 && parsedFileType !== 2)
        throw new Error(`unsuppurted file type ${fileType}`);
    const createExpenseUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Mapper/CreateMappedExpenses?fileName=${fileName}&fileType=${parsedFileType}&userID=${userId}&chargedDate=${chargedDate}`
    console.log(`the create url is ${createExpenseUrl}`);

    try {
      const token = cookies.get('token');
      console.log(`the token is ${token}`);      
      const response = await axios.post(createExpenseUrl, null, {
        headers: {
            'Authorization': `Bearer ${token}`,
        }
    });
      return response.data;
    } catch (error) {
      console.error('create expense error:', error);
      throw new Error('create expense failed');
    }
  };

  const getMonthlyExpenses = async (chargedDate:Date, userId: number) => {
    console.log(`the user id on getMonthly is ${userId}`);
    const month = chargedDate.getMonth();
    const year = chargedDate.getFullYear();
    const MonthlyExpensesUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Mapper/GetMappedExpensesPerMonth?Month=${month}&Year=${year}&userId=${userId}`
    console.log(`the monthly expense url is ${MonthlyExpensesUrl}`);

    try{
        const token = cookies.get('token');
        const response = await axios.get(MonthlyExpensesUrl, {
          headers: {
            'Authorization': `Bearer ${token}`,   
          }  
        });
        console.log(response.data);
        return response.data;       
    } catch(error){
        console.error('get expenses error:', error);
        throw new Error('get expenses failed');
    }
}

const mapperService={
    createMappedExpenses,
    getMonthlyExpenses
}

export default mapperService;