using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManager.Integrations.GetExpensesAPI;
using ExpensesManager.Integrations.SplitWiseModels;
using ExpensesManger.Services.BuisnessLogic.Map;
using Users = ExpensesManager.Integrations.SplitWiseModels.Users;

namespace ExpensesManger.Services
{
    public class SplitewiseExpenseService : ISplitwiseExpensesService
    {
        #region Const/ Members

        private const int INVALID_DIGIT = -1;

        private readonly AppDbContext appDbContext;

        #endregion

        #region Properties
        public SplitwiseExpenses SplitWiseExpenseDO { get; set; }
        public SwRecords SwRecords { get; set; }

        #endregion

        #region Ctor
        public SplitewiseExpenseService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            SplitWiseExpenseDO = new SplitwiseExpenses();
            SwRecords = new SwRecords();
        }

        #endregion

        #region ISplitewiseExpenseServiceImpl/Public Methods

        public List<SwRecords> GetAllSwRecords()
        {
            return appDbContext.SpliteWise.ToList();
        }

        public List<SwRecords> GetSwRecords(DateTime fromDate)
        {
            return appDbContext.SpliteWise.Where(sw => sw.Linked_Month == fromDate.Month.ToString()).ToList();
        }

        public List<SwRecords> CreateSwRecords(DateTime fromDate)
        {
            SplitwiseExpenses splitwiseExpenses = CallingSplitwiseGetExpensesApi(fromDate);
            List<SwRecords> swRecordItems = new();

            foreach (SplitwiseExpense expense in splitwiseExpenses.Expenses)
            {
                swRecordItems.AddRange(ParseSwExpenseToSwRecords(expense));
            }

            foreach (var swRecord in swRecordItems)
            {
                appDbContext.Add(swRecord);
                appDbContext.SaveChanges();
            }

            return swRecordItems;
        }

        public void DeleteSwRecords(DateTime fromDate)
        {
            List<SwRecords> swRecordsToDelete = appDbContext.SpliteWise.Where(sw => sw.Linked_Month == fromDate.Month.ToString()).ToList();
            foreach (var swRecordToDelete in swRecordsToDelete)
            {
                appDbContext.Remove(swRecordToDelete);
                appDbContext.SaveChanges();
            }
        }

        #endregion
        
        #region Private Methods
        private SplitwiseExpenses CallingSplitwiseGetExpensesApi(DateTime fromDate)
        {
            Task<string> runApiTask = Task.Run(() => GetExpensesExecuter.SplitWiseApiHandlerWithDates(fromDate));
            runApiTask.Wait();

            return SplitWiseExpenseDO.ParseResponseToExpensesObj(runApiTask.Result);
        }
        private List<SwRecords> ParseSwExpenseToSwRecords(SplitwiseExpense splitwiseExpenses)
        {
            Users userPaid = splitwiseExpenses.Users.FirstOrDefault(user => user.User.ID == splitwiseExpenses.Created_By.id);
            Users userOwed = splitwiseExpenses.Users.FirstOrDefault(user => user.User.ID != splitwiseExpenses.Created_By.id);
            List<SwRecords> swRecords = new List<SwRecords>();

            var swRecordsPaid = new SwRecords()
            {
                Internal_TransactionID = Utils.GenerateRandomID(),
                SW_TransactionID = splitwiseExpenses.ID,
                SW_User_ID = userPaid == null ? INVALID_DIGIT : userPaid.User_ID,
                Total_Cost = Convert.ToDouble(splitwiseExpenses.Cost),
                Creation_Method = splitwiseExpenses.Creation_Method,
                Paid_Share = userPaid == null ? INVALID_DIGIT : Convert.ToDouble(userPaid.Paid_Share),
                Owed_Share = userPaid == null ? INVALID_DIGIT : Convert.ToDouble(userPaid.Owed_Share),
                Expense_Creation_Date = splitwiseExpenses.Created_At.ToString(),
                Record_Creation_Date = DateTime.Now.ToString(),
                Expense_Description = splitwiseExpenses.Description,
                Linked_Month = Utils.RegexMatcherMonthToCharge(splitwiseExpenses.Description, Utils.GetUserChargeDay(userPaid.User_ID), splitwiseExpenses.Created_At).ToString(),
                Category = CategoryExpenseMapper.GetCategoryMapping(splitwiseExpenses.Description).ToString()
            };

            var swRecordsOwed = new SwRecords()
            {
                Internal_TransactionID = Utils.GenerateRandomID(),
                SW_TransactionID = splitwiseExpenses.ID,
                SW_User_ID = userOwed == null ? INVALID_DIGIT : userOwed.User_ID,
                Total_Cost = Convert.ToDouble(splitwiseExpenses.Cost),
                Creation_Method = splitwiseExpenses.Creation_Method,
                Paid_Share = userOwed == null ? INVALID_DIGIT : Convert.ToDouble(userOwed.Paid_Share),
                Owed_Share = userOwed == null ? INVALID_DIGIT : Convert.ToDouble(userOwed.Owed_Share),
                Expense_Creation_Date = splitwiseExpenses.Created_At.ToString(),
                Record_Creation_Date = DateTime.Now.ToString(),
                Expense_Description = splitwiseExpenses.Description,
                Linked_Month = Utils.RegexMatcherMonthToCharge(splitwiseExpenses.Description, Utils.GetUserChargeDay(userPaid.User_ID), splitwiseExpenses.Created_At).ToString(),
                Category = CategoryExpenseMapper.GetCategoryMapping(splitwiseExpenses.Description).ToString()
            };

            swRecords.Add(swRecordsPaid);
            swRecords.Add(swRecordsOwed);

            return swRecords;
        } 
        #endregion
    }
}
