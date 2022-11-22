using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;


namespace ExpensesManger.Services
{
    public class RecalculatedExpenseService : IRecalculatedExpenseService
    {
        #region Const
        
        private const int DEFAULT_DAY = 1;
        private const int LOGGED_IN_USER_ID = 19773792;

        #endregion

        #region Member
        
        private readonly AppDbContext m_AppDbContext;

        #endregion
        
        #region Ctor

        public RecalculatedExpenseService(AppDbContext appDbContext)
        {
            this.m_AppDbContext = appDbContext;
        }

        #endregion
        #region IRecalculatedExpenseService Methods

        /// <summary>
        /// Pre condition - mapped expense and sw expense exist in the DB
        /// </summary>
        /// <param name="fromDate"> the month and year we want to calculate</param>
        /// <returns> list with new expense that were created </returns>
        public List<RecalculatedExpenseRecord> CreateRecalculatedExpenseRecords(DateTime fromDate)
        {
            List<SwRecords> swRecords = m_AppDbContext.SpliteWise.Where(sw => sw.Linked_Month == fromDate.Month.ToString()).ToList();
            List<ExpenseRecord> expensesRecords = m_AppDbContext.Expenses.Where(ex => ex.Linked_Month == fromDate.Month.ToString()).ToList();
            List<RecalculatedExpenseRecord> recalculatedExpenseRecords = new();

            List<ExpenseRecord> expensesInCategory = new();

            foreach (SwRecords swRecord in swRecords)
            {
                if (swRecord.SW_User_ID == LOGGED_IN_USER_ID)
                {
                    expensesInCategory = expensesRecords.Where(c => c.Category == swRecord.Category).ToList();
                }

                RecalculatedExpenseRecord recalculatedItem = CreateNewRecalculatedExpenseItems(expensesInCategory, swRecord);
                m_AppDbContext.Add(recalculatedItem);
                m_AppDbContext.SaveChanges();
                recalculatedExpenseRecords.Add(recalculatedItem);
            }

            return recalculatedExpenseRecords;
        }

        public void DeleteRecalculatedExpenseRecords(DateTime fromDate)
        {
            List<RecalculatedExpenseRecord> recalculatedExpenseRecords = m_AppDbContext.RecalculatedExpenseRecords.Where(rer => rer.Linked_Month == fromDate.Month.ToString()
                                                                                                                   && rer.Linked_Year == fromDate.Year.ToString()).ToList();
            foreach (var recalculatedExpenseRecord in recalculatedExpenseRecords)
            {
                m_AppDbContext.Remove(recalculatedExpenseRecord);
                m_AppDbContext.SaveChanges();
            }
        }

        public void DeleteRecalculatedExpenseRecord(int recalculatedExpenseRecordID)
        {
            RecalculatedExpenseRecord? recalculatedExpenseRecord = m_AppDbContext.RecalculatedExpenseRecords.FirstOrDefault(r => r.Recalculated_Expense_Record_Trans_ID == recalculatedExpenseRecordID);

            if (recalculatedExpenseRecord != null)
            {
                m_AppDbContext.Remove(recalculatedExpenseRecord);
                m_AppDbContext.SaveChanges();
            }
        }

        public RecalculatedExpenseRecord EditRecalculatedExpenseRecord(RecalculatedExpenseRecord recalculatedExpenseForEdit)
        {
            var currRecalculatedExpenseRecords = m_AppDbContext.RecalculatedExpenseRecords
                                                .FirstOrDefault(rer => rer.ExpenseRecord_TransactionID == recalculatedExpenseForEdit.ExpenseRecord_TransactionID);

            currRecalculatedExpenseRecords.Recalculated_Expense_Record_Trans_ID = recalculatedExpenseForEdit.Recalculated_Expense_Record_Trans_ID;
            currRecalculatedExpenseRecords.ExpenseRecord_TransactionID = recalculatedExpenseForEdit.ExpenseRecord_TransactionID;
            currRecalculatedExpenseRecords.SW_TransactionID = recalculatedExpenseForEdit.SW_TransactionID;
            currRecalculatedExpenseRecords.SW_UserID = recalculatedExpenseForEdit.SW_UserID;
            currRecalculatedExpenseRecords.Expense_Description = recalculatedExpenseForEdit.Expense_Description;
            currRecalculatedExpenseRecords.Linked_Month = recalculatedExpenseForEdit.Linked_Month;
            currRecalculatedExpenseRecords.Linked_Year = recalculatedExpenseForEdit.Linked_Year;
            currRecalculatedExpenseRecords.Expense_Creation_Date = recalculatedExpenseForEdit.Expense_Creation_Date;
            currRecalculatedExpenseRecords.Record_Creation_Date = recalculatedExpenseForEdit.Record_Creation_Date;
            currRecalculatedExpenseRecords.Category = currRecalculatedExpenseRecords.Category;
            currRecalculatedExpenseRecords.Owed_Share = recalculatedExpenseForEdit.Owed_Share;
            currRecalculatedExpenseRecords.Paid_Share = recalculatedExpenseForEdit.Paid_Share;

            m_AppDbContext.SaveChanges();

            return currRecalculatedExpenseRecords;
        }

        public List<RecalculatedExpenseRecord> GetAllRecalculatedExpenseRecords()
        {
            return m_AppDbContext.RecalculatedExpenseRecords.ToList();
        }

        public List<RecalculatedExpenseRecord> GetRecalculatedExpenseRecords(DateTime fromDate)
        {
            return m_AppDbContext.RecalculatedExpenseRecords.Where(rer => rer.Linked_Month == fromDate.Month.ToString()).ToList();
        }

        #endregion

        #region Private Methods
        
        private RecalculatedExpenseRecord CreateNewRecalculatedExpenseItems(List<ExpenseRecord> expensesInCategory, SwRecords swRecord)
        {
            ExpenseRecord expenseItemToRecalculate = new ExpenseRecord();
            DateTime expenseCreateDate = new DateTime();
            RecalculatedExpenseRecord recalculatedExpenseRecord;

            recalculatedExpenseRecord = new RecalculatedExpenseRecord()
            {
                Recalculated_Expense_Record_Trans_ID = DateUtils.GenerateRandomID(),
                SW_TransactionID = swRecord.SW_TransactionID,
                SW_UserID = swRecord.SW_User_ID,
                Expense_Description = swRecord.Expense_Description,
                Linked_Month = swRecord.Linked_Month,
                Linked_Year = swRecord.Linked_Year,
                Record_Creation_Date = DateTime.Now.ToString(),
                Owed_Share = swRecord.Owed_Share,
                Paid_Share = swRecord.Paid_Share,
            };

            if (expensesInCategory != null)
            {
                expenseItemToRecalculate = expensesInCategory.FirstOrDefault(expense => Math.Round(expense.Debit_Amount) == Math.Round(swRecord.Total_Cost));
            }

            else
            {
                DateTime.TryParse(swRecord.Expense_Creation_Date, out expenseCreateDate);
            }

            if (expenseItemToRecalculate != null)
            {
                recalculatedExpenseRecord.ExpenseRecord_TransactionID = expenseItemToRecalculate.TransactionID;
                recalculatedExpenseRecord.Expense_Creation_Date = expenseItemToRecalculate.Transaction_Date;
                recalculatedExpenseRecord.Category = expenseItemToRecalculate.Category;
            }

            else
            {

                recalculatedExpenseRecord.ExpenseRecord_TransactionID = DateUtils.GenerateRandomID();
                recalculatedExpenseRecord.Expense_Creation_Date = new DateTime(Convert.ToInt32(swRecord.Linked_Year), Convert.ToInt32(swRecord.Linked_Month), DEFAULT_DAY).ToString();
                recalculatedExpenseRecord.Category = swRecord.Category;
            }

            return recalculatedExpenseRecord;
        }

        #endregion
    }
}
