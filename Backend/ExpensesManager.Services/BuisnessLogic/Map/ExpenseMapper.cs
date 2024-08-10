using System.Data;
using ExpensesManager.BuisnessLogic.Core;
using System.Reflection;
using Newtonsoft.Json;
using ExpensesManager.Services.Map.Models;
using ExpensesManager.Services.Map.Common;
using ExpensesManager.Services.BuisnessLogic.Map;
using ExpensesManager.DB.Models;
using ExpensesManager.Services.BuisnessLogic.Map.Common;
using ExpensesManager.Services.BuisnessLogic.Map.ExpenseMappers;
using ExpensesManager.Services;

namespace ExpensesManager.Services
{
    public class ExpenseMapper
    {
        private readonly IServiceProvider _serviceProvider;

        #region Ctors
        public ExpenseMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ExpenseMapper()
        {
        }
        #endregion


        #region Properties
        [DataNames("CardInfo")]
        public string CardDetails { get; set; }

        [DataNames("TransDate")]
        public DateTime? TransactionDate { get; set; }

        [DataNames("Desc")]
        public string ExpenseDescription { get; set; }

        [DataNames("Price")]
        public double PriceAmount { get; set; }

        [DataNames("Debit")]
        public double DebitAmount { get; set; }


        [DataNames("ForegnierDebitAmount")]
        public string ForegnierDebitAmount { get; set; }

        [DataNames("DebitCurrency")]
        public string DebitCurrency { get; set; }

        [DataNames("PriceCurrency")]
        public string PriceCurrency { get; set; }

        [DataNames("ExchangeDescription")]
        public string ExchangeDescription { get; set; }

        public double ExchangeRate { get; set; }

        public double ExchangeCommission { get; set; }

        public CategoryExpenseMapper CategoryData { get; set; }
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// the methods mapping the file into the constant categories
        /// </summary>
        /// <param name="dataTable">the raw data from the file</param>
        /// <param name="fileType"> expenses, movment or other </param>
        /// <returns>List with mapped categories </returns>
        public List<ExpenseMapper> MapFile(DataTable dataTable, BankTypes.FileTypes fileType, int userId, ExpenseMapperFactory expenseMapperFactory)
        {
            NamingColumns(dataTable, fileType, expenseMapperFactory);
            DataNamesMapper<ExpenseMapper> namesMapper = new DataNamesMapper<ExpenseMapper>();

            List<ExpenseMapper>? mappedListWithColumnNames = namesMapper.Map(dataTable).ToList();
            List<ExpenseMapper> mappedList = new List<ExpenseMapper>();

            foreach (ExpenseMapper? mappedRow in mappedListWithColumnNames)
            {
                if (!string.IsNullOrEmpty(mappedRow.ExpenseDescription) && mappedRow.PriceAmount > 0)
                {
                    mappedRow.CategoryData = expenseMapperFactory.GetMapper<CategoryExpenseMapper>(_serviceProvider);
                    mappedRow.CategoryData.CategoryKey = mappedRow.CategoryData.GetCategoryMapping(mappedRow.ExpenseDescription, userId);
                    mappedList.Add(mappedRow);
                }
            }

            return mappedList;
        }

        public List<ExpenseRecord> GetExpensesPerMonth(List<ExpenseRecord> distinctCategoryList, int currentExpenseChargeDate, int userID)
        {
            List<ExpenseRecord> filteredList = distinctCategoryList.Where(item => item.Price_Amount > 0).ToList();

            return filteredList.Where(er => er.Linked_Month == currentExpenseChargeDate.ToString() && er.User_ID == userID).ToList();
        }

        public List<ExpenseRecord> GetExpensesePerMonthAndYear(List<ExpenseRecord> distinctCategoryList, int currentExpenseMonth, int currentExpenseYear, int userID)
        {
            var expensesPerMonth = GetExpensesPerMonth(distinctCategoryList, currentExpenseMonth, userID);
            return expensesPerMonth.Where(e => e.Linked_Year == currentExpenseYear.ToString()).ToList();
        }

        #endregion Public Methods

        #region Virtual Methods

        public virtual DataTable CustomNamingColumns(DataTable dataTable)
        {
            throw new NotImplementedException("CustomNamingColumns method must be overridden in a derived class.");
        }

        #endregion

        #region Private Methods

        private void NamingColumns(DataTable dataTable, BankTypes.FileTypes fileType, ExpenseMapperFactory expenseMapperFactory)
        {
            ExpenseMapper mapperType = expenseMapperFactory.CreateExpenseMapper(fileType);
            mapperType.CustomNamingColumns(dataTable);
        }

        #endregion
    }
}
