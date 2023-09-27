import axios from 'axios';

const createSplitwiseExpenses = async (fromDate: string) => {   
    const swCreateUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/SplitewiseExpenses/CreateSplitewiseExpenseRecords?fromDate=${fromDate}`
    try {
      const response = await axios.post(swCreateUrl);
      return response.data;
    } catch (error) {
      console.error('create sw expense error:', error);
      throw new Error('create sw expense failed');
    }
  };

const getSplitwiseExpenses = async (fromDate: string) => {
    const swGetUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/SplitewiseExpenses/GetSwRecords?fromDate=${fromDate}`;
    try {
        const response = await axios.get(swGetUrl);
        return response.data;
      } catch (error) {
        console.error('create sw expense error:', error);
        throw new Error('create sw expense failed');
      }

}  
const splitewiseExpenseService={
    createSplitwiseExpenses,
    getSplitwiseExpenses
}

export default splitewiseExpenseService;