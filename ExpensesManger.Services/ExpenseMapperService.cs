using ExpensesManager.Services;
using ExpensesManager.DB;
using System.Data;
using System.Linq;
using ExpensesManager.DB.Models;
using ExpensesManager.BuisnessLogic.Core;
using ExpensesManger.Services.BuisnessLogic.Map;

namespace ExpensesManger.Services
{
    public class ExpenseMapperService : IExpenseMapperService
    {
        private const int CURRENT_USER_ID = 19773792;// Will be moved after regi/logic implemntation
        private readonly AppDbContext appDbContext;
        public ExpenseMapper Mapper { get; private set; }
        public CategoryExpenseMapper CategoryExpense{ get; set; }

        #region Ctor

        public ExpenseMapperService(AppDbContext context)
        {
            appDbContext = context;
            Mapper = new ExpenseMapper();
            CategoryExpense = new CategoryExpenseMapper();
        }

        #endregion
        
        public List<ExpenseRecord> GetMapExpenses()
        {
            return appDbContext.Expenses.ToList();
        }
        
        public void DeleteExpenses(DateTime currentExpenseMonth)
        {            
            var expensesPerMonth = Mapper.GetExpensesPerMonth(appDbContext.Expenses.ToList(), currentExpenseMonth, CURRENT_USER_ID);
            foreach (var expense in expensesPerMonth)
            {
                appDbContext.Remove(expense);
                appDbContext.SaveChanges();
            }
        }

        public List<ExpenseRecord> CreateExpenses(DataTable dataTable, ExpenseMapper.FileTypes fileType)
        {
            List<ExpenseMapper>? mappedExpenses = Mapper.MapFile(dataTable, fileType);

            foreach (var expense in mappedExpenses)
            {
                appDbContext.Add(SetExpenseMapperToExpenseRecord(expense));
                appDbContext.SaveChanges();
            }

            return appDbContext.Expenses.ToList();
        }

        public ExpenseRecord EditExpense(ExpenseRecord editedExpense,int expenseID)
        {
            var expense = appDbContext.Expenses.FirstOrDefault(e=> e.TransactionID == expenseID);
            
            expense = editedExpense;
            appDbContext.SaveChanges();

            return expense;
        }

        internal static ExpenseRecord SetExpenseMapperToExpenseRecord(ExpenseMapper expenseMapper)
        {
            return new ExpenseRecord()
            {
                TransactionID = Utils.GenerateRandomID(),
                Transaction_Date = expenseMapper.Transaction_Date.ToString(),
                Card_Details = expenseMapper.Card_Details,
                Record_Create_Date = DateTime.Now.ToString(),
                Expense_Description = expenseMapper.Expense_Description,
                Price_Amount = expenseMapper.Price_Amount,
                Debit_Amount = expenseMapper.Debit_Amount,
                Currency = expenseMapper.Currency,
                Exchange_Rate = expenseMapper.Exchange_Rate,
                Exchange_Description = expenseMapper.Exchange_Description,
                Category = expenseMapper.CategoryData.Category.ToString(),
                User_ID = CURRENT_USER_ID,
                Linked_Month = Utils.GetExpenseLinkedMonth(expenseMapper.Transaction_Date.Value, Utils.GetUserChargeDay(CURRENT_USER_ID)).ToString()
            };
        }


    }
}