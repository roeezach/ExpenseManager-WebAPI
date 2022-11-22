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
            HouseComitee,
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
        public Dictionary<CategoryGroup,List<ExpenseRecord>> AppendExpenseRecordToCategories(List<ExpenseRecord> mappedList)
        {
            Dictionary<CategoryGroup, List<ExpenseRecord>> categorisedExpensesDict = new Dictionary<CategoryGroup, List<ExpenseRecord>>();
            double categoriesSum = 0;

            foreach (int category in Enum.GetValues(typeof(CategoryGroup)))
            {
                ExpensesInCategory = new List<ExpenseRecord>();
                
                foreach (ExpenseRecord item in mappedList)
                {
                    AddItemsToExpensesInCategoryList(item, ((CategoryGroup)category).ToString());
                }

                categoriesSum = ExpensesInCategory.Sum(cat => cat.Debit_Amount);
                if (categoriesSum == 0)// in case no purchase were made for this category
                        Category = (CategoryGroup)category;

                categorisedExpensesDict.Add((CategoryGroup)category, ExpensesInCategory);
            }

            return categorisedExpensesDict;
        }

        public static CategoryGroup GetCategoryMapping(string transactionName)
        {
            // will replace this with DB call to each user definitions - the list is in the debug folder
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
        /// <param name="createdExpense"></param>
        /// <param name="currentExpense"></param>
        /// <param name="category"></param>
        private void AddItemsToExpensesInCategoryList(ExpenseRecord currentExpense, string category)
        {
            bool isForeignTransaction = false;
            ExpenseRecord createdExpense = new();


            if (currentExpense.Category == category &&!string.IsNullOrEmpty(currentExpense.Expense_Description) && currentExpense.Expense_Description != TITLE_DESCRIPTION)
            {
                if (string.IsNullOrEmpty(createdExpense.Expense_Description))
                {
                    createdExpense.Category = currentExpense.Category;
                    createdExpense.Expense_Description = currentExpense.Expense_Description;
                    createdExpense.Transaction_Date= currentExpense.Transaction_Date;
                    createdExpense.Currency = currentExpense.Currency;
                    createdExpense.Record_Create_Date = createdExpense.Record_Create_Date;
                    createdExpense.User_ID = currentExpense.User_ID;
                    createdExpense.Linked_Month = currentExpense.Linked_Month;
                }

                if (!string.IsNullOrEmpty(createdExpense.Exchange_Description))
                {
                    isForeignTransaction = true;
                    createdExpense.Price_Amount = currentExpense.Price_Amount.Value;
                    createdExpense.Debit_Amount = currentExpense.Price_Amount.Value;
                    createdExpense.Exchange_Rate = DateUtils.RegexMatcherForgeignCurrency(currentExpense.Expense_Description);
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
