import axios from 'axios';
import { fileTypeParser } from '../utils/FileTypes';

// const fileTypeParser = (fileType:string)=> {
//     if(fileType === 'Habinleumi')
//         return 0;
//     else if(fileType === 'Hapoalim')
//         return 1;
//     else if(fileType === 'Max')
//         return 2;
//   }
  

const createMappedExpenses = async (fileName: string, fileType:string, chargedDate: string) => {
    const userId = parseInt(process.env.REACT_APP_USER_ID_TEMP, 10); // TODO - USER MANAGMENT
    const parsedFileType = fileTypeParser(fileType);
    console.log(`parsed file type was ${parsedFileType}`);       
    if(parsedFileType !== 0 && parsedFileType !== 1 && parsedFileType !== 2)
        throw new Error(`unsuppurted file type ${fileType}`);
    const createExpenseUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Mapper/CreateMappedExpenses?fileName=${fileName}&fileType=${parsedFileType}&userID=${userId}&chargedDate=${chargedDate}`
    console.log(`the create url is ${createExpenseUrl}`);

    try {
      const response = await axios.post(createExpenseUrl);
      return response.data;
    } catch (error) {
      console.error('create expense error:', error);
      throw new Error('create expense failed');
    }
  };


  const getMonthlyExpenses = async (chargedDate:Date) => {
    const month = chargedDate.getMonth();
    const year = chargedDate.getFullYear();
    const userId = parseInt(process.env.REACT_APP_USER_ID_TEMP, 10); // TODO - USER MANAGMENT
    const MonthlyExpensesUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Mapper/GetMappedExpensesPerMonth?Month=${month}&Year=${year}&userId=${userId}`
    console.log(`the monthly expense url is ${MonthlyExpensesUrl}`);

    try{
        const response = await axios.get(MonthlyExpensesUrl);
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