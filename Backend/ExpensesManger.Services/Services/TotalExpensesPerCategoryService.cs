using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.BuisnessLogic.Map;
using ExpensesManger.Services.Contracts;

namespace ExpensesManger.Services.Services
{
    public class TotalExpensesPerCategoryService : ITotalExpensesPerCategoryService
    {
        #region Const

        private const int INVALID_DIGIT = -1;
        private const int NO_MONEY = 0;
        private const int NO_ITEMS = 0;

        #endregion

        #region Member

        private readonly AppDbContext m_AppDbContext;
        private readonly ICategoryService m_CategoryService;

        #endregion

        #region Ctor

        public TotalExpensesPerCategoryService(AppDbContext context, ICategoryService categoryService)
        {
            m_AppDbContext = context;
            m_CategoryService = categoryService;
        }

        #endregion

        #region ITotalExpensesPerCategory Impl/ Public Methods

        public Dictionary<string, double> GetCategoriesSum(int month, int year, int userID)
        {
            return m_AppDbContext.TotalExpensesPerCategory.Where(c => c.Year == year && c.Month == month && c.SW_UserID == userID && c.Total_Amount > 0)
                   .ToDictionary(key => key.Category, value => value.Total_Amount);
        }

        public double GetCategorySum(int month, int year, string category, int userID)
        {
            TotalExpensePerCategory? expensePerCategory = m_AppDbContext.TotalExpensesPerCategory.FirstOrDefault(c => c.Category == category && c.Year == year
                                                                                                                        && c.Month == month && c.SW_UserID == userID);
            return expensePerCategory == null ? -1 : expensePerCategory.Total_Amount;
        }

        public double GetTotalExpensesSum(int month, int year, int userId)
        {
            var expensePerCategory = m_AppDbContext.TotalExpensesPerCategory.Where(total => total.Year == year && total.Month == month && total.SW_UserID == userId).ToList();

            return expensePerCategory == null ? INVALID_DIGIT : expensePerCategory.Sum(catSum => catSum.Total_Amount);
        }


        public List<TotalExpensePerCategory> GetTotalCategories()
        {
            return m_AppDbContext.TotalExpensesPerCategory.ToList();
        }

        public TotalExpensePerCategory EditTotalExpensePerCategory(TotalExpensePerCategory expensePerCategory, string category, int month, int year)
        {
            TotalExpensePerCategory? editedTotalExpensePerCategory = m_AppDbContext.TotalExpensesPerCategory.FirstOrDefault(c => c.Category == category && c.Year == year && c.Month == month);
            editedTotalExpensePerCategory.Category = expensePerCategory.Category;
            editedTotalExpensePerCategory.SW_UserID = expensePerCategory.SW_UserID;
            editedTotalExpensePerCategory.Month = expensePerCategory.Month;
            editedTotalExpensePerCategory.Year = expensePerCategory.Year;
            editedTotalExpensePerCategory.Total_Amount = expensePerCategory.Total_Amount;
            m_AppDbContext.SaveChanges();

            return editedTotalExpensePerCategory;

        }

        /// <summary>
        /// pre-condition: for the given month and year we have expense record and split wise record in the DB
        /// </summary>
        /// <param name="montlyExpenses"></param>
        /// <returns></returns>
        public Dictionary<string, List<ExpenseRecord>> CreateTotalExpensesPerCategory(List<ExpenseRecord> montlyExpenses, DateTime fromDate, int userID)
        {
            CategoryExpenseMapper categoryMapper = new(m_CategoryService);
            Dictionary<string, List<ExpenseRecord>> expensesCategories = categoryMapper.AppendExpenseRecordToCategories(montlyExpenses, userID);

            foreach (KeyValuePair<string, List<ExpenseRecord>> categoryItem in expensesCategories)
            {
                if (categoryItem.Value.Any())
                {
                    var expensePerCategory = CreateExpensePerCategoryForExpenseRecord(categoryItem.Key, categoryItem.Value, fromDate, userID);
                    m_AppDbContext.Add(expensePerCategory);
                    m_AppDbContext.SaveChanges();
                }
                var uniqueRecEx = m_AppDbContext.RecalculatedExpenseRecords.Where(item => item.Category == categoryItem.Key
                                                                                          && categoryItem.Value.Count == NO_ITEMS && item.Owed_Share > NO_MONEY && item.SW_UserID == userID)
                                                                                         .ToList();
                if (uniqueRecEx.Any())
                {
                    TotalExpensePerCategory? uniqueTotalExpenses = CreateTepcForToUniqeRecalculatedExpense(categoryItem.Key, uniqueRecEx, fromDate, userID);
                    if(uniqueTotalExpenses != null)
                    {
                        m_AppDbContext.Add(uniqueTotalExpenses);
                        m_AppDbContext.SaveChanges();
                    }
                }
            }

            return expensesCategories;
        }

        public void DeleteExpensePerCategory(TotalExpensePerCategory expensePerCategory)
        {
            m_AppDbContext.Remove(expensePerCategory);
            m_AppDbContext.SaveChanges();
        }

        public void DeleteAllTotalExpensesPerTimePeriod(DateTime fromDate, int userID)
        {
            List<TotalExpensePerCategory> totalExpenses = m_AppDbContext.TotalExpensesPerCategory.Where(tec => tec.Month == fromDate.Month
                                                                           && tec.Year == fromDate.Year && tec.SW_UserID == userID).ToList();
            if (totalExpenses.Any())
            {
                foreach (var totalExpense in totalExpenses)
                {
                    m_AppDbContext.Remove(totalExpense);
                    m_AppDbContext.SaveChanges();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// pre condition:  for the given month and year we have expense record,split wise record and reclculated expense record in the DB
        /// </summary>
        /// <param name="category"></param>
        /// <param name="montlyExpensesInCateogry"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private TotalExpensePerCategory CreateExpensePerCategoryForExpenseRecord(string category, List<ExpenseRecord> montlyExpensesInCateogry, DateTime fromDate, int userId)
        {
            IEnumerable<ExpenseRecord> ItemsWithValue = montlyExpensesInCateogry.Where(t => !string.IsNullOrEmpty(t.Transaction_Date));
            DateTime dateOfCategory = ItemsWithValue != null ? DateTime.Parse(ItemsWithValue.FirstOrDefault().Transaction_Date) : DateTime.MinValue;
            List<RecalculatedExpenseRecord> recalculatedExpenses = GetRecalculatedExpenseRecordPerCategory(category, fromDate, userId);
            TotalExpensePerCategory totalExpensePerCategory = new()
            {
                ItemID = Utils.GenerateRandomID(),
                Total_Amount = recalculatedExpenses.Any() ? RecalaculatedExpenseCategoriesHandler(recalculatedExpenses, montlyExpensesInCateogry, userId) : montlyExpensesInCateogry.Sum(x => x.Debit_Amount),
                Category = category.ToString(),
                SW_UserID = userId,
                Month = Convert.ToInt32(montlyExpensesInCateogry.FirstOrDefault().Linked_Month),
                Year = dateOfCategory.Year
            };

            return totalExpensePerCategory;
        }

        /// <summary>        
        /// TODO: Use reflection and make this and CreateExpensePerCategoryForExpenseRecord genric. take general obj. 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="montlyExpensesInCateogry"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private TotalExpensePerCategory? CreateTepcForToUniqeRecalculatedExpense(string category, List<RecalculatedExpenseRecord> montlyExpensesInCateogry, DateTime fromDate, int userID)
        {
            List<RecalculatedExpenseRecord> recalculatedExpenses = GetRecalculatedExpenseRecordPerCategory(category, fromDate, userID);

            List<RecalculatedExpenseRecord>? recalculatedExpensesCurrUser = recalculatedExpenses.Where(u => u.SW_UserID == userID)
                                                                            .ToList();
            if(recalculatedExpensesCurrUser == null || recalculatedExpensesCurrUser.Count == 0)
                    return null;

            TotalExpensePerCategory totalExpensePerCategory = new()
            {
                ItemID = Utils.GenerateRandomID(),
                Total_Amount = recalculatedExpensesCurrUser.Sum(x => x.Owed_Share),
                Category = category,
                SW_UserID = userID,
                Month = fromDate.Month,
                Year = fromDate.Year
            };

            return totalExpensePerCategory;
        }

        private List<RecalculatedExpenseRecord> GetRecalculatedExpenseRecordPerCategory(string category, DateTime fromDate, int userID)
        {
            return m_AppDbContext.RecalculatedExpenseRecords.Where(rer => rer.Linked_Month == fromDate.Month.ToString()
                                                                                                                        && rer.Linked_Year == fromDate.Year.ToString()
                                                                                                                        && rer.Category == category
                                                                                                                        && rer.SW_UserID == userID)
                                                                                                                        .ToList();
        }

        private double RecalaculatedExpenseCategoriesHandler(List<RecalculatedExpenseRecord> recalculatedExpenseInCurrentCategory, List<ExpenseRecord> montlyExpensesInCateogry, int userId)
        {
            double currMonthlyExpensesInCategory = montlyExpensesInCateogry.Sum(x => x.Debit_Amount);

            var recExpCategoryCurrentUser = recalculatedExpenseInCurrentCategory.Where(id => id.SW_UserID == userId);

            foreach (RecalculatedExpenseRecord recalculatedExpenseRecord in recExpCategoryCurrentUser)
            {
                if (recalculatedExpenseRecord.Paid_Share > NO_MONEY)
                {
                    if (currMonthlyExpensesInCategory == NO_MONEY)
                        currMonthlyExpensesInCategory = recalculatedExpenseRecord.Owed_Share;
                    else
                        currMonthlyExpensesInCategory -= recalculatedExpenseRecord.Owed_Share;
                }

                else
                    currMonthlyExpensesInCategory += recalculatedExpenseRecord.Owed_Share;
            }

            return currMonthlyExpensesInCategory;
        }
    }

    #endregion
}
