
using ExpensesManager.DB.Models;

namespace ExpensesManger.Services
{
    public interface IRecalculatedExpenseService
    {
        List<RecalculatedExpenseRecord> GetAllRecalculatedExpenseRecords();
        List<RecalculatedExpenseRecord> GetRecalculatedExpenseRecords(DateTime fromDate);
        RecalculatedExpenseRecord EditRecalculatedExpenseRecord(RecalculatedExpenseRecord recalculatedExpenseForEdit);
        List<RecalculatedExpenseRecord> CreateRecalculatedExpenseRecords(DateTime fromDate);
        void DeleteRecalculatedExpenseRecords(DateTime fromDate);
        void DeleteRecalculatedExpenseRecord(int recalculatedExpenseRecordID);
    }
}
