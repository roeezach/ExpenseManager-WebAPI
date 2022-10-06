using ExpensesManager.Services;
using System.Data;
using ExpensesManager.DB.Models;

namespace ExpensesManger.Services 
{
    public interface IExpenseMapperService
    {        
        List<ExpenseRecord> GetMapExpenses();
        List<ExpenseRecord> CreateExpenses(DataTable dataTable, ExpenseMapper.FileTypes fileType);
        ExpenseRecord EditExpense(ExpenseRecord editedExpense, int expenseID);
        void DeleteExpenses(DateTime currentExpenseMonth);
    }
}
