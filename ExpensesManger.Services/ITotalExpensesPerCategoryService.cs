using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map;

namespace ExpensesManger.Services
{
    public interface ITotalExpensesPerCategoryService
    {
        Dictionary<string,double > GetCategoriesSum(int month, int year);
        double GetCategorySum(int month, int year, string category);
        List<TotalExpensePerCategory> GetCategories();
        TotalExpensePerCategory EditTotalExpensePerCategory(TotalExpensePerCategory expensePerCategory, string category, int month, int year);
        Dictionary<CategoryExpenseMapper.CategoryGroup, List<ExpenseRecord>> CreateTotalExpensesPerCategory(List<ExpenseRecord> montlyExpenses);
        void DeleteExpensePerCategory(TotalExpensePerCategory expensePerCategory);
    }
}
