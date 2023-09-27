import axios from 'axios';

const getRecalculateExpenses = async (fromDate: string) => {   
    const rcCreateUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/RecalculateExpense/GetRecalculatedExpenseRecords?fromDate=${fromDate}`
    try {
      const response = await axios.get(rcCreateUrl);
      return response.data;
    } catch (error) {
      console.error('create sw expense error:', error);
      throw new Error('create sw expense failed');
    }
  };

const createRecalculateExpenses = async (fromDate: string) => {

    const rcGetUrl = `${process.env.REACT_APP_BACKEND_BASE_URL}/RecalculateExpense/CreateRecalculatedExpenseRecords?fromDate=${fromDate}`;
    try {
        const response = await axios.post(rcGetUrl);
        return response.data;
      } catch (error) {
        console.error('create sw expense error:', error);
        throw new Error('create sw expense failed');
      }

}  

const recalculateExpenseService={
    createRecalculateExpenses,
    getRecalculateExpenses
}

export default recalculateExpenseService;