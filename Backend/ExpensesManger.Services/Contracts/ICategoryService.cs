using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map;

namespace ExpensesManger.Services.Contracts
{
    public interface ICategoryService
    {
        Categories CreateCategory(Categories category);
        void DeleteCategory(Categories deletedCategory);
        Categories EditCategory(Categories category);
        List<Categories> GetCategories();
        Categories GetUserCategories(int userID);

    }
}
