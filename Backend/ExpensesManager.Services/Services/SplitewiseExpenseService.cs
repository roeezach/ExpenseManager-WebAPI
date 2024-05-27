using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManager.Integrations.GetExpensesAPI;
using ExpensesManager.Integrations.SplitWiseModels;
using ExpensesManager.Services.BuisnessLogic.Map;
using ExpensesManager.Services.Contracts;
using Users = ExpensesManager.Integrations.SplitWiseModels.Users;
using Microsoft.Extensions.Configuration;
using ExcelDataReader.Log;
using System.Diagnostics;

namespace ExpensesManager.Services.Services
{
    public class SplitewiseExpenseService : ISplitwiseExpensesService
    {
        #region Const/ Members

        private const int INVALID_DIGIT = -1;
        private const string LAST_MONTH_IN_YEAR = "12";
        private const int ONE_YEAR = 1;
        private readonly AppDbContext appDbContext;
        private readonly ICategoryService m_CategoryService;
        private readonly IConfiguration m_configuration;


        #endregion

        #region Properties
        public SplitwiseExpenses SplitWiseExpenseDO { get; set; }
        public SwRecords SwRecords { get; set; }

        #endregion

        #region Ctor
        public SplitewiseExpenseService(AppDbContext appDbContext, ICategoryService categoryService, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            m_configuration = configuration;
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
            return appDbContext.SpliteWise.Where(sw => sw.Linked_Month == fromDate.Month.ToString()
            && sw.Linked_Year == fromDate.Year.ToString())
            .ToList();
        }

        public List<SwRecords> CreateSwRecords(DateTime fromDate)
        {
            SplitwiseExpenses splitwiseExpenses = CallingSplitwiseGetExpensesApi(fromDate);
            List<SwRecords> swRecordItems = new();

            var existingSwRecordsAtMonth = appDbContext.SpliteWise.Where(t => t.Linked_Month == fromDate.Month.ToString()
            && t.Linked_Year == fromDate.Year.ToString());

            foreach (SplitwiseExpense expense in splitwiseExpenses.Expenses)
            {
                var existingExpense = existingSwRecordsAtMonth.Where(item => item.Expense_Description == expense.Description).ToList();
                if (!existingExpense.Any())
                {
                    swRecordItems.AddRange(ParseSwExpenseToSwRecords(expense));
                }
            }

            if (swRecordItems.Any())
            {
                appDbContext.AddRange(swRecordItems);
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
            Task<string> runApiTask = Task.Run(() => GetExpensesExecuter.SplitWiseApiHandlerWithDates(fromDate, m_configuration));
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
            Users swUserPaid = splitwiseExpenses.Users.FirstOrDefault(user => user.User.ID == splitwiseExpenses.Created_By.id);
            Users swUserOwed = splitwiseExpenses.Users.FirstOrDefault(user => user.User.ID != splitwiseExpenses.Created_By.id) ?? splitwiseExpenses.Users.FirstOrDefault();
            var internalPaidUserId = appDbContext.Users.FirstOrDefault(user => user.SW_User_ID == swUserPaid.User_ID.ToString()).UserID;
            var internalOwedUserId = appDbContext.Users.FirstOrDefault(user => user.SW_User_ID == swUserOwed.User_ID.ToString()).UserID;

            var dateToCharge = Utils.RegexMatcherDateToCharge(splitwiseExpenses.Description, Utils.GetUserChargeDay(internalPaidUserId, appDbContext), splitwiseExpenses.Created_At);

            List<SwRecords> swRecords = new List<SwRecords>();
            CategoryExpenseMapper categoryExpenseMapper = new(m_CategoryService);

            var swRecordsPaid = new SwRecords()
            {
                Internal_TransactionID = Utils.GenerateRandomID(),
                SW_TransactionID = splitwiseExpenses.ID,
                SW_User_ID = swUserPaid == null ? INVALID_DIGIT : swUserPaid.User_ID,
                Total_Cost = Convert.ToDouble(splitwiseExpenses.Cost),
                Creation_Method = splitwiseExpenses.Creation_Method,
                Paid_Share = swUserPaid == null ? INVALID_DIGIT : Convert.ToDouble(swUserPaid.Paid_Share),
                Owed_Share = swUserPaid == null ? INVALID_DIGIT : Convert.ToDouble(swUserPaid.Owed_Share),
                Expense_Creation_Date = splitwiseExpenses.Created_At.ToString(),
                Record_Creation_Date = DateTime.Now.ToString(),
                Expense_Description = splitwiseExpenses.Description,
                Linked_Month = dateToCharge.Month.ToString(),
                Linked_Year = dateToCharge.Year.ToString(),
            };
            swRecordsPaid.Category = categoryExpenseMapper.GetCategoryMapping(splitwiseExpenses.Description, internalPaidUserId).ToString();

            dateToCharge = Utils.RegexMatcherDateToCharge(splitwiseExpenses.Description, Utils.GetUserChargeDay(swUserOwed.User_ID, appDbContext), splitwiseExpenses.Created_At);

            var swRecordsOwed = new SwRecords()
            {
                Internal_TransactionID = Utils.GenerateRandomID(),
                SW_TransactionID = splitwiseExpenses.ID,
                SW_User_ID = swUserOwed == null ? INVALID_DIGIT : swUserOwed.User_ID,
                Total_Cost = Convert.ToDouble(splitwiseExpenses.Cost),
                Creation_Method = splitwiseExpenses.Creation_Method,
                Paid_Share = swUserOwed == null ? INVALID_DIGIT : Convert.ToDouble(swUserOwed.Paid_Share),
                Owed_Share = swUserOwed == null ? INVALID_DIGIT : Convert.ToDouble(swUserOwed.Owed_Share),
                Expense_Creation_Date = splitwiseExpenses.Created_At.ToString(),
                Record_Creation_Date = DateTime.Now.ToString(),
                Expense_Description = splitwiseExpenses.Description,
                Linked_Month = dateToCharge.Month.ToString(),
                Linked_Year = dateToCharge.Year.ToString(),
            };
            swRecordsOwed.Category = categoryExpenseMapper.GetCategoryMapping(splitwiseExpenses.Description, internalOwedUserId).ToString();


            swRecords.Add(swRecordsPaid);
            swRecords.Add(swRecordsOwed);

            return swRecords;
        }
        #endregion
    }
}
