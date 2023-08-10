using ExpensesManager.Services;
using ExpensesManager.DB;
using System.Data;
using System.Linq;
using ExpensesManager.DB.Models;
using ExpensesManager.BuisnessLogic.Core;
using ExpensesManger.Services.BuisnessLogic.Map;
using ExpensesManger.Services.BuisnessLogic.Map.Common;
using ExpensesManger.Services.Contracts;

namespace ExpensesManger.Services.Services
{
    public class ExpenseMapperService : IExpenseMapperService
    {

        #region Members
        private readonly AppDbContext m_AppDbContext;
        private readonly ICategoryService m_CategoryService;
        private readonly IExpenseMapperFactory m_ExpenseMapperFactory;
        private readonly IServiceProvider m_ServiceProvider;

        #endregion
        public ExpenseMapper Mapper { get; private set; }
        public CategoryExpenseMapper CategoryExpense { get; set; }

        #region Ctor

        public ExpenseMapperService(AppDbContext context, ICategoryService categoryService, IExpenseMapperFactory expenseMapperFactory, IServiceProvider serviceProvider)
        {
            m_AppDbContext = context;
            m_ExpenseMapperFactory = expenseMapperFactory;
            m_CategoryService = categoryService;
            m_ServiceProvider = serviceProvider;
            Mapper = new ExpenseMapper(serviceProvider);
            CategoryExpense = ExpenseMapperFactory.GetMapper<CategoryExpenseMapper>(m_ServiceProvider);
        }

        #endregion

        #region Public Methods

        public List<ExpenseRecord> GetMapExpenses()
        {
            return m_AppDbContext.Expenses.ToList();
        }

        public void DeleteExpenses(int currentExpenseMonth, int currentExpenseYear, int userID)
        {
            var expensesPerMonth = Mapper.GetExpensesPerMonth(m_AppDbContext.Expenses.ToList(), currentExpenseMonth, userID);
            expensesPerMonth = expensesPerMonth.Where(e => e.Linked_Year == currentExpenseYear.ToString()).ToList();

            foreach (var expense in expensesPerMonth)
            {
                m_AppDbContext.Remove(expense);
                m_AppDbContext.SaveChanges();
            }
        }

        public List<ExpenseRecord> CreateExpenses(DataTable dataTable, BankTypes.FileTypes fileType, int userID, DateTime ChargeDate)
        {
            List<ExpenseMapper>? mappedExpenses = Mapper.MapFile(dataTable, fileType, userID, m_ExpenseMapperFactory);

            foreach (var expense in mappedExpenses)
            {
                m_AppDbContext.Add(SetExpenseMapperToExpenseRecord(expense, userID, ChargeDate));
                m_AppDbContext.SaveChanges();
            }

            return m_AppDbContext.Expenses.ToList();
        }

        public ExpenseRecord EditExpense(ExpenseRecord editedExpense, int expenseID)
        {
            var expense = m_AppDbContext.Expenses.FirstOrDefault(e => e.TransactionID == expenseID);

            expense = editedExpense;
            m_AppDbContext.SaveChanges();

            return expense;
        }
               
        #endregion

        #region Internal Methods

        internal static ExpenseRecord SetExpenseMapperToExpenseRecord(ExpenseMapper expenseMapper, int userID, DateTime chargedDate)
        {
            string month = Utils.GetExpenseLinkedMonth(expenseMapper.TransactionDate.Value, chargedDate).ToString();
            string? exchangeDescription = Utils.ReformatHebrewString(expenseMapper.ExchangeDescription);

            ExpenseRecord expenseRecord = new()
            {
                TransactionID = Utils.GenerateRandomID(),
                Transaction_Date = expenseMapper.TransactionDate.ToString(),
                Card_Details = expenseMapper.CardDetails,
                Record_Create_Date = DateTime.Now.ToString(),
                Expense_Description = expenseMapper.ExpenseDescription,
                Price_Amount = expenseMapper.PriceAmount,
                Debit_Amount = expenseMapper.DebitAmount,
                DebitCurrency = expenseMapper.DebitCurrency,
                Exchange_Rate = expenseMapper.ExchangeRate,
                Exchange_Description = exchangeDescription,
                Category = expenseMapper.CategoryData.CategoryKey,
                Linked_Month = month,
                User_ID = userID,
                Linked_Year = Utils.GetExpenseLinkedYearAsDateTime(expenseMapper.TransactionDate.Value, Utils.GetUserChargeDay(userID), Convert.ToInt32(month)).Year.ToString()
            };

            if (!string.IsNullOrEmpty(expenseMapper.ExchangeDescription))
            {
                expenseRecord.Debit_Amount = double.Parse(expenseMapper.ForegnierDebitAmount);
                expenseRecord.Exchange_Rate = Utils.GetExchangeRate(exchangeDescription);
            }

            return expenseRecord;
        }
        #endregion
    }
}