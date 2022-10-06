using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB.Models;
using ExpensesManager.Services;
using ExpensesManager.Services.Map.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace ExpensesManger.Services.BuisnessLogic.Map
{
    public class CategoryExpenseMapper
    {
        public const string TITLE_DESCRIPTION = "שם  העסק";

        #region Enum
        public enum CategoryGroup
        {
            Supermarket = 1,
            Rent,
            HealthTreatment,
            Transport,
            InternetAndTV,
            NotMappedYet,
            HouseComittee,
            Pubs,
            Resturant,
            Electricity,
            Gas,
            Water,
            Fines,
            AtmAndCommissions,
            Health,
            Pharm,
            Parking,
            ConvenienceStore,
            Vaction,
            Movies,
            Furniture,
            Gym,
            DontationMusicAndStroage,
            ForeignerCurrency,
            Electronics,
            Bit,
            School,
            MunicipalRate
        }

        public CategoryGroup Category { get; set; }
        public List<ExpenseRecord> ExpensesInCategory { get; private set; }


        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mappedList"></param>
        /// <returns></returns>
        public Dictionary<CategoryGroup,List<ExpenseRecord>> SumCategories(List<ExpenseRecord> mappedList)
        {
            Dictionary<CategoryGroup, List<ExpenseRecord>> categorisedExpensesDict = new Dictionary<CategoryGroup, List<ExpenseRecord>>();

            foreach (int category in Enum.GetValues(typeof(CategoryGroup)))
            {
                ExpenseRecord expenseWithCategroy = new();
                ExpensesInCategory = new List<ExpenseRecord>();
                
                foreach (ExpenseRecord item in mappedList)
                {
                    expenseWithCategroy = AddItemsToExpensesInCategory(expenseWithCategroy, item, ((CategoryGroup)category).ToString());
                }
                if (expenseWithCategroy.Debit_Amount == 0)// in case no purchase were made for this category
                        Category = (CategoryGroup)category;

                categorisedExpensesDict.Add((CategoryGroup)category, ExpensesInCategory);
            }

            return categorisedExpensesDict;
        }

        public static CategoryGroup GetCategoryMapping(string transactionName)
        {
            // will replace this with DB call to each user definitions
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"MappedCategoties.json");

            List<MappedCategoryNames> mappedCategories = JsonConvert.DeserializeObject<List<MappedCategoryNames>>(File.ReadAllText(path));

            foreach (MappedCategoryNames mappedCategory in mappedCategories)
            {
                bool isKeyWordFound = mappedCategory.Keywords.Any(name => transactionName.Contains(name));

                if (isKeyWordFound)
                {
                    return (CategoryGroup)Enum.Parse(typeof(CategoryGroup), mappedCategory.CategoryName);
                }
            }

            return CategoryGroup.NotMappedYet;
        }

        public List<ExpenseMapper> MapListToCategories(List<ExpenseMapper> mappedExpenses)
        {
            foreach (ExpenseMapper mappedRow in mappedExpenses)
            {
                if (!string.IsNullOrEmpty(mappedRow.Expense_Description) && mappedRow.Expense_Description != TITLE_DESCRIPTION)
                    Category = GetCategoryMapping(mappedRow.Expense_Description);
            }

            return mappedExpenses;

        }

        #region Private Method

        /// <summary>
        /// test this functionality after refactor
        /// </summary>
        /// <param name="candidateExpenseToAdd"></param>
        /// <param name="currentExpense"></param>
        /// <param name="category"></param>
        private ExpenseRecord AddItemsToExpensesInCategory(ExpenseRecord candidateExpenseToAdd, ExpenseRecord currentExpense, string category)
        {
            bool isForeignTransaction = false;

            if (currentExpense.Category == category &&!string.IsNullOrEmpty(currentExpense.Expense_Description) && currentExpense.Expense_Description != TITLE_DESCRIPTION)
            {
                if (string.IsNullOrEmpty(candidateExpenseToAdd.Expense_Description))
                {
                    candidateExpenseToAdd.Category = currentExpense.Category;
                    candidateExpenseToAdd.Expense_Description = currentExpense.Expense_Description;
                    candidateExpenseToAdd.Transaction_Date= currentExpense.Transaction_Date;
                    candidateExpenseToAdd.Currency = currentExpense.Currency;
                    candidateExpenseToAdd.Record_Create_Date = candidateExpenseToAdd.Record_Create_Date;
                    candidateExpenseToAdd.User_ID = currentExpense.User_ID;
                    candidateExpenseToAdd.Linked_Month = currentExpense.Linked_Month;
                }

                if (!string.IsNullOrEmpty(candidateExpenseToAdd.Exchange_Description))
                {
                    isForeignTransaction = true;
                    candidateExpenseToAdd.Price_Amount = currentExpense.Price_Amount.Value; // it was += => need to make sure the amounts are correct
                    candidateExpenseToAdd.Debit_Amount = currentExpense.Price_Amount.Value;
                    candidateExpenseToAdd.Exchange_Rate = Utils.RegexMatcherForgeignCurrency(currentExpense.Expense_Description);
                }

                if (!isForeignTransaction)
                {
                    candidateExpenseToAdd.Debit_Amount = currentExpense.Debit_Amount;
                    candidateExpenseToAdd.Price_Amount = currentExpense.Price_Amount;
                }

                ExpensesInCategory.Add(candidateExpenseToAdd);
            }

            return candidateExpenseToAdd;
        } 

        #endregion
    }
}
