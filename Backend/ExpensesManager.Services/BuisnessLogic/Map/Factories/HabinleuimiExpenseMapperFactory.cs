using ExpensesManager.Services;
using ExpensesManager.Services.BuisnessLogic.Map;
using ExpensesManager.Services.BuisnessLogic.Map.ExpenseMappers;

public class HabinleuimiExpenseMapperFactory : IExpenseMapperFactory
{
    public ExpenseMapper CreateExpenseMapper()
    {
        return new HabinleuimiExpenseMapper();
    }
}