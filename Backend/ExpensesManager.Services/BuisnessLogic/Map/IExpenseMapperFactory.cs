using ExpensesManager.Services;
using ExpensesManager.Services.BuisnessLogic.Map.Common;

namespace ExpensesManager.Services.BuisnessLogic.Map
{
    public interface IExpenseMapperFactory
    {
        ExpenseMapper GetBankMapper(BankTypes.FileTypes fileType);

    }
}
