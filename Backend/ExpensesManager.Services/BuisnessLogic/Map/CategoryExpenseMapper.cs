using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManager.Services.Map.Models;
using Newtonsoft.Json;
using System.Reflection;
using ExpensesManager.Services.Contracts;

namespace ExpensesManager.Services.BuisnessLogic.Map
{
    public class CategoryExpenseMapper
    {
        #region Constants and Members
        public const string TITLE_DESCRIPTION = "שם  העסק";
        private const string NOT_MAPPED_YET = "NotMappedYet";
        private readonly ICategoryService m_CategoryService;

        #endregion

        #region Ctor
        public CategoryExpenseMapper(ICategoryService categoryService)
        {
            m_CategoryService = categoryService;
        }

        #endregion

        #region Properties

        public string CategoryKey { get; set; }
        public List<ExpenseRecord> ExpensesInCategory { get; private set; }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mappedList"></param>
        /// <returns></returns>
        public Dictionary<string, List<ExpenseRecord>> AppendExpenseRecordToCategories(List<ExpenseRecord> mappedList, int userId)
        {
            Dictionary<string, List<ExpenseRecord>> categorisedExpensesDict = new Dictionary<string, List<ExpenseRecord>>();
            List<MappedCategoryNames>? mappedCategories = JsonConvert.DeserializeObject<List<MappedCategoryNames>>(m_CategoryService.GetUserCategories(userId).MappedCategoriesJson);
            foreach (MappedCategoryNames category in mappedCategories)
            {
                ExpensesInCategory = new List<ExpenseRecord>();
                CategoryKey = category.CategoryName;

                foreach (ExpenseRecord item in mappedList)
                {
                    AddItemsToExpensesInCategoryList(item, category.CategoryName, userId);
                }
                categorisedExpensesDict.Add(category.CategoryName, ExpensesInCategory);
            }

            return categorisedExpensesDict;
        }

        public string GetCategoryMapping(string transactionName, int userID)
        {
            List<MappedCategoryNames>? mappedCategories = JsonConvert.DeserializeObject<List<MappedCategoryNames>>(m_CategoryService.GetUserCategories(userID).MappedCategoriesJson);

            foreach (MappedCategoryNames mappedCategory in mappedCategories)
            {
                bool isKeyWordFound = mappedCategory.Keywords.Any(name => transactionName.Replace(" ", "").ToLower().Contains(name.ToLower()));

                if (isKeyWordFound)
                {
                    return mappedCategory.CategoryName;
                }
            }

            return NOT_MAPPED_YET;
        }

        #region Private Method

        /// <summary>
        /// test this functionality after refactor
        /// </summary>
        /// <param name="createdExpense"></param>
        /// <param name="currentExpense"></param>
        /// <param name="category"></param>
        private void AddItemsToExpensesInCategoryList(ExpenseRecord currentExpense, string category, int userID)
        {
            bool isForeignTransaction = false;
            ExpenseRecord createdExpense = new();


            if (currentExpense.Category == category && currentExpense.Debit_Amount > 0 && currentExpense.User_ID == userID)
            {
                if (string.IsNullOrEmpty(createdExpense.Expense_Description))
                {
                    createdExpense.Category = currentExpense.Category;
                    createdExpense.Expense_Description = currentExpense.Expense_Description;
                    createdExpense.Transaction_Date = currentExpense.Transaction_Date;
                    createdExpense.DebitCurrency = currentExpense.DebitCurrency;
                    createdExpense.Record_Create_Date = createdExpense.Record_Create_Date;
                    createdExpense.User_ID = currentExpense.User_ID;
                    createdExpense.Linked_Month = currentExpense.Linked_Month;
                }

                if (!string.IsNullOrEmpty(createdExpense.Exchange_Description))
                {
                    isForeignTransaction = true;
                    createdExpense.Price_Amount = currentExpense.Price_Amount.Value;
                    createdExpense.Debit_Amount = currentExpense.Price_Amount.Value;
                    createdExpense.Exchange_Rate = Utils.RegexMatcherForgeignCurrency(currentExpense.Expense_Description);
                    createdExpense.PriceCurrency = currentExpense.PriceCurrency;
                    createdExpense.Exchange_Description = Utils.ReformatHebrewString(createdExpense.Exchange_Description);
                }

                if (!isForeignTransaction)
                {
                    createdExpense.Debit_Amount += currentExpense.Debit_Amount;
                    createdExpense.Price_Amount += currentExpense.Price_Amount;
                }

                ExpensesInCategory.Add(createdExpense);
            }
        }

        #endregion
    }
}
