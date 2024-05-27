using ExpensesManager.Services;

namespace ExpensesManager.BuisnessLogic.Core
{
    public class AppFactory
    {
        public ExpenseMapper ExpenseMapper
        { 
            get
            {
                return new ExpenseMapper();
            } 
        }
    }
}
