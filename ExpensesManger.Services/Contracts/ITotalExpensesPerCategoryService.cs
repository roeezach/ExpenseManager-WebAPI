using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map;

namespace ExpensesManger.Services.Contracts
{
    public interface ITotalExpensesPerCategoryService
    {
        Dictionary<string, double> GetCategoriesSum(int month, int year, int userID);
        double GetCategorySum(int month, int year, string category, int userID);
        double GetTotalExpensesSum(int month, int year, int userID);
        List<TotalExpensePerCategory> GetTotalCategories();
        TotalExpensePerCategory EditTotalExpensePerCategory(TotalExpensePerCategory expensePerCategory, string category, int month, int year);
        Dictionary<string, List<ExpenseRecord>> CreateTotalExpensesPerCategory(List<ExpenseRecord> montlyExpenses, DateTime fromDate, int userID);
        void DeleteExpensePerCategory(TotalExpensePerCategory expensePerCategory);
        void DeleteAllTotalExpensesPerTimePeriod(DateTime fromDate, int userID);
    }
}
