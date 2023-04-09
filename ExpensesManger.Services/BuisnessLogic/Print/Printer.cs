using ExpensesManager.Services;
using ExpensesManger.Services.BuisnessLogic.Map;

namespace ExpensesManager.BuisnessLogic.Print
{
    public class Printer
    {
        internal void PrintInfo(List<ExpenseMapper> distinctCategoryData, List<ExpenseMapper> mappedData)
        {
            Console.WriteLine("\n\n\n expense for the given month are: ");
            foreach (ExpenseMapper item in distinctCategoryData)
            {
                if (item.DebitAmount != 0)
                {
                    Console.WriteLine($"the debit is {item.DebitAmount} for category {item.CategoryData}");
                }
            }

            PrintUnmappedData(mappedData);

            double totalExpenses = distinctCategoryData.Sum(x => x.DebitAmount);
            Console.WriteLine($"\n the total expenses of the given month is {totalExpenses}");
        }

        private void PrintUnmappedData(List<ExpenseMapper> mappedData)
        {
            Console.WriteLine("\n unmapped expenses below \n");

            IEnumerable<ExpenseMapper> notMappedCollection = mappedData.Where(item => item.CategoryData.CategoryKey == "Not Mapped Yet");

            foreach (ExpenseMapper unMapItem in notMappedCollection)
            {
                if (unMapItem.TransactionDate != null)
                    Console.WriteLine($"the debit is {unMapItem.PriceAmount} and the transaction date was on {unMapItem.TransactionDate} for the category {unMapItem.CategoryData}");
            }

            Console.WriteLine($"\n total expense of unmapped data is {notMappedCollection.Sum(x => x.DebitAmount)}");

            Console.WriteLine("\n foreigen currency expenses bellow : \n");
            IEnumerable<ExpenseMapper> foreignCurrCollection = mappedData.Where(item => item.CategoryData.CategoryKey == "ForeignerCurrency");

            foreach (ExpenseMapper foreignCurrItem in foreignCurrCollection)
            {
                if (foreignCurrItem.TransactionDate != null)
                    Console.WriteLine($"the debit is {foreignCurrItem.PriceAmount} and the transaction date was on {foreignCurrItem.TransactionDate} for the category {foreignCurrItem.CategoryData}");
            }

        }
    }
}
