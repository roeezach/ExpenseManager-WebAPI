
using ExpensesManager.DB.Models;

namespace ExpensesManger.Services.Contracts
{
    public interface ISplitwiseExpensesService
    {
        List<SwRecords> GetAllSwRecords();
        List<SwRecords> GetSwRecords(DateTime fromDate);
        List<SwRecords> CreateSwRecords(DateTime fromDate);
        void DeleteSwRecords(DateTime fromDate);
    }
}
