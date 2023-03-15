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
        private const string LAST_MONTH_IN_YEAR = "12";
        private const int ONE_YEAR = 1;
        private readonly AppDbContext appDbContext;
        private readonly ICategoryService m_CategoryService;


        #endregion

        #region Properties
        public SplitwiseExpenses SplitWiseExpenseDO { get; set; }
        public SwRecords SwRecords { get; set; }

        #endregion

        #region Ctor
        public SplitewiseExpenseService(AppDbContext appDbContext, ICategoryService categoryService)
        {
            this.appDbContext = appDbContext;
            SplitWiseExpenseDO = new SplitwiseExpenses();
            SwRecords = new SwRecords();
            m_CategoryService = categoryService;
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

        /// <summary>
        /// 
        /// TODO: because of this method should seperate utils and make date utils not static
        /// </summary>
        /// <param name="splitwiseExpenses"></param>
        /// <returns></returns>
        private List<SwRecords> ParseSwExpenseToSwRecords(SplitwiseExpense splitwiseExpenses)
        {
            Users userPaid = splitwiseExpenses.Users.FirstOrDefault(user => user.User.ID == splitwiseExpenses.Created_By.id);
            Users userOwed = splitwiseExpenses.Users.FirstOrDefault(user => user.User.ID != splitwiseExpenses.Created_By.id) ?? splitwiseExpenses.Users.FirstOrDefault();

            var dateToCharge = DateUtils.RegexMatcherDateToCharge(splitwiseExpenses.Description, DateUtils.GetUserChargeDay(userPaid.User_ID), splitwiseExpenses.Created_At);

            List<SwRecords> swRecords = new List<SwRecords>();
            CategoryExpenseMapper categoryExpenseMapper = new(m_CategoryService);

            var swRecordsPaid = new SwRecords()
            {
                Internal_TransactionID = DateUtils.GenerateRandomID(),
                SW_TransactionID = splitwiseExpenses.ID,
                SW_User_ID = userPaid == null ? INVALID_DIGIT : userPaid.User_ID,
                Total_Cost = Convert.ToDouble(splitwiseExpenses.Cost),
                Creation_Method = splitwiseExpenses.Creation_Method,
                Paid_Share = userPaid == null ? INVALID_DIGIT : Convert.ToDouble(userPaid.Paid_Share),
                Owed_Share = userPaid == null ? INVALID_DIGIT : Convert.ToDouble(userPaid.Owed_Share),
                Expense_Creation_Date = splitwiseExpenses.Created_At.ToString(),
                Record_Creation_Date = DateTime.Now.ToString(),
                Expense_Description = splitwiseExpenses.Description,
                Linked_Month = dateToCharge.Month.ToString(),
                Linked_Year = dateToCharge.Year.ToString(),
            };
            swRecordsPaid.Category = categoryExpenseMapper.GetCategoryMapping(splitwiseExpenses.Description, swRecordsPaid.SW_User_ID).ToString();


            dateToCharge = DateUtils.RegexMatcherDateToCharge(splitwiseExpenses.Description, DateUtils.GetUserChargeDay(userOwed.User_ID), splitwiseExpenses.Created_At);

            var swRecordsOwed = new SwRecords()
            {
                Internal_TransactionID = DateUtils.GenerateRandomID(),
                SW_TransactionID = splitwiseExpenses.ID,
                SW_User_ID = (userOwed == null) ? INVALID_DIGIT : userOwed.User_ID,
                Total_Cost = Convert.ToDouble(splitwiseExpenses.Cost),
                Creation_Method = splitwiseExpenses.Creation_Method,
                Paid_Share = (userOwed == null) ? INVALID_DIGIT : Convert.ToDouble(userOwed.Paid_Share),
                Owed_Share = (userOwed == null) ? INVALID_DIGIT : Convert.ToDouble(userOwed.Owed_Share),
                Expense_Creation_Date = splitwiseExpenses.Created_At.ToString(),
                Record_Creation_Date = DateTime.Now.ToString(),
                Expense_Description = splitwiseExpenses.Description,
                Linked_Month = dateToCharge.Month.ToString(),                
                Linked_Year = dateToCharge.Year.ToString(),
            };
            swRecordsOwed.Category = categoryExpenseMapper.GetCategoryMapping(splitwiseExpenses.Description, swRecordsOwed.SW_User_ID).ToString();


            swRecords.Add(swRecordsPaid);
            swRecords.Add(swRecordsOwed);

            return swRecords;
        } 
        #endregion
    }
}
