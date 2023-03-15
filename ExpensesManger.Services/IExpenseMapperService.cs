using ExpensesManager.Services;
using System.Data;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.BuisnessLogic.Map.Common;

namespace ExpensesManger.Services 
{
    public interface IExpenseMapperService
    {        
        List<ExpenseRecord> GetMapExpenses();
        List<ExpenseRecord> CreateExpenses(DataTable dataTable, BankTypes.FileTypes fileType,int userID);
        ExpenseRecord EditExpense(ExpenseRecord editedExpense, int expenseID);
        void DeleteExpenses(DateTime currentExpenseMonth, int userID);
    }
}
