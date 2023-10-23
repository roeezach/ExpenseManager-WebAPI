import axios from 'axios';
import { fileTypeParser } from '../utils/FileTypes';
import { useAuth } from '../context/AuthContext';

const createMappedExpenses = async (fileName: string, fileType:string, chargedDate: string) => {
    const { user } = useAuth();
    const parsedFileType = fileTypeParser(fileType);
    console.log(`parsed file type was ${parsedFileType}`);       
    if(parsedFileType !== 0 && parsedFileType !== 1 && parsedFileType !== 2)
        throw new Error(`unsuppurted file type ${fileType}`);
    const createExpenseUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Mapper/CreateMappedExpenses?fileName=${fileName}&fileType=${parsedFileType}&userID=${user.userID}&chargedDate=${chargedDate}`
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
    const { user } = useAuth();
    const month = chargedDate.getMonth();
    const year = chargedDate.getFullYear();
    const MonthlyExpensesUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/Mapper/GetMappedExpensesPerMonth?Month=${month}&Year=${year}&userId=${user.userID}`
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