using ExpensesManager.Services;
using ExpensesManager.DB;
using System.Data;
using System.Linq;
using ExpensesManager.DB.Models;
using ExpensesManager.BuisnessLogic.Core;
using ExpensesManger.Services.BuisnessLogic.Map;
using ExpensesManger.Services.BuisnessLogic.Map.Common;

namespace ExpensesManger.Services
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
        public CategoryExpenseMapper CategoryExpense{ get; set; }

        #region Ctor

        public ExpenseMapperService(AppDbContext context, ICategoryService categoryService,IExpenseMapperFactory expenseMapperFactory, IServiceProvider serviceProvider)
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

        public void DeleteExpenses(DateTime currentExpenseMonth, int userID)
        {
            var expensesPerMonth = Mapper.GetExpensesPerMonth(m_AppDbContext.Expenses.ToList(), currentExpenseMonth, userID);
            foreach (var expense in expensesPerMonth)
            {
                m_AppDbContext.Remove(expense);
                m_AppDbContext.SaveChanges();
            }
        }

        public List<ExpenseRecord> CreateExpenses(DataTable dataTable, BankTypes.FileTypes fileType,int userID)
        {
            List<ExpenseMapper>? mappedExpenses = Mapper.MapFile(dataTable, fileType,userID, m_ExpenseMapperFactory);

            foreach (var expense in mappedExpenses)
            {
                m_AppDbContext.Add(SetExpenseMapperToExpenseRecord(expense, userID));
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

        internal static ExpenseRecord SetExpenseMapperToExpenseRecord(ExpenseMapper expenseMapper, int userID)
        {
            string month = DateUtils.GetExpenseLinkedMonth(expenseMapper.Transaction_Date.Value, DateUtils.GetUserChargeDay(userID)).ToString();

            ExpenseRecord expenseRecord =  new()
            {
                TransactionID = DateUtils.GenerateRandomID(),
                Transaction_Date = expenseMapper.Transaction_Date.ToString(),
                Card_Details = expenseMapper.Card_Details,
                Record_Create_Date = DateTime.Now.ToString(),
                Expense_Description = expenseMapper.Expense_Description,
                Price_Amount = expenseMapper.Price_Amount,
                Debit_Amount = expenseMapper.Debit_Amount,
                Currency = expenseMapper.Currency,
                Exchange_Rate = expenseMapper.Exchange_Rate,
                Exchange_Description = expenseMapper.Exchange_Description,
                Category = expenseMapper.CategoryData.CategoryKey,
                Linked_Month = month,
                User_ID = userID,
                Linked_Year = DateUtils.GetExpenseLinkedYearAsDateTime(expenseMapper.Transaction_Date.Value, DateUtils.GetUserChargeDay(userID), Convert.ToInt32(month)).Year.ToString()
        };

            return expenseRecord;
        } 
        #endregion
    }
}