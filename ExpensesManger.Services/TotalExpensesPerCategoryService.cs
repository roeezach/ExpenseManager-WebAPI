using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map;


namespace ExpensesManger.Services
{
    public class TotalExpensesPerCategoryService : ITotalExpensesPerCategoryService
    {
        private readonly AppDbContext appDbContext;
        public TotalExpensesPerCategoryService(AppDbContext context)
        {
            appDbContext = context;
        }
        
        public Dictionary<string, double> GetCategoriesSum(int month, int year)
        {
            return appDbContext.TotalExpensesPerCategory.Where(c => (c.Year == year && c.Month == month)&& c.Total_Amount > 0)
                   .ToDictionary(key => key.Category,value => value.Total_Amount);
        }

        public double GetCategorySum(int month, int year, string category)
        {
            TotalExpensePerCategory expensePerCategory = appDbContext.TotalExpensesPerCategory.FirstOrDefault(c => (c.Category == category && c.Year == year && c.Month == month));
            return expensePerCategory == null ? -1 : expensePerCategory.Total_Amount;
        }

        public List<TotalExpensePerCategory> GetTotalCategories()
        {
            return appDbContext.TotalExpensesPerCategory.ToList();
        }

        public TotalExpensePerCategory EditTotalExpensePerCategory(TotalExpensePerCategory expensePerCategory, string category, int month, int year)
        {
            TotalExpensePerCategory? editedTotalExpensePerCategory = appDbContext.TotalExpensesPerCategory.FirstOrDefault(c => (c.Category == category && c.Year == year && c.Month == month));
            editedTotalExpensePerCategory.Category = expensePerCategory.Category;
            editedTotalExpensePerCategory.SW_UserID = expensePerCategory.SW_UserID;
            editedTotalExpensePerCategory.Month = expensePerCategory.Month;
            editedTotalExpensePerCategory.Year = expensePerCategory.Year;
            editedTotalExpensePerCategory.Total_Amount = expensePerCategory.Total_Amount;
            appDbContext.SaveChanges();

            return editedTotalExpensePerCategory;

        }
        public Dictionary<CategoryExpenseMapper.CategoryGroup, List<ExpenseRecord>> CreateTotalExpensesPerCategory(List<ExpenseRecord> montlyExpenses)
        {
            CategoryExpenseMapper categoryMapper = new CategoryExpenseMapper();
            Dictionary<CategoryExpenseMapper.CategoryGroup, List<ExpenseRecord>> expensesCategories = categoryMapper.SumCategories(montlyExpenses);

            foreach (KeyValuePair<CategoryExpenseMapper.CategoryGroup, List<ExpenseRecord>> categoryItem in expensesCategories)
            {
                if(categoryItem.Value.Any())
                {
                    TotalExpensePerCategory expensePerCategory = CreateTotalExpensePerCategoryInstance(categoryItem.Key, categoryItem.Value);
                    appDbContext.Add(expensePerCategory);
                    appDbContext.SaveChanges();
                }
            }
            return expensesCategories;
        }

        public void DeleteExpensePerCategory(TotalExpensePerCategory expensePerCategory)
        {           
            appDbContext.Remove(expensePerCategory);
            appDbContext.SaveChanges();
        }

        private TotalExpensePerCategory CreateTotalExpensePerCategoryInstance(CategoryExpenseMapper.CategoryGroup category, List<ExpenseRecord> montlyExpensesInCateogry)
        {            
                IEnumerable<ExpenseRecord> ItemsWithValue = montlyExpensesInCateogry.Where(t => !string.IsNullOrEmpty(t.Transaction_Date));
                DateTime dateOfCategory = ItemsWithValue != null ? DateTime.Parse(ItemsWithValue.FirstOrDefault().Transaction_Date) : DateTime.MinValue;

                TotalExpensePerCategory totalExpensePerCategory = new()
                {
                    ItemID = Utils.GenerateRandomID(),
                    Total_Amount = montlyExpensesInCateogry.Sum(x => x.Debit_Amount),
                    Category = category.ToString(),
                    SW_UserID = 19773792, //to be changed after Loign and user managment implementation
                    Month = Convert.ToInt32(montlyExpensesInCateogry.FirstOrDefault().Linked_Month),
                    Year = dateOfCategory.Year
                };

                return totalExpensePerCategory;            
        }

    }
}
