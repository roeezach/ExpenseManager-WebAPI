using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map.Common;

namespace ExpensesManger.Services.BuisnessLogic.Map
{
    public interface IExpenseMapperFactory
    {
        ExpenseMapper GetBankMapper(BankTypes.FileTypes fileType);

    }
}
