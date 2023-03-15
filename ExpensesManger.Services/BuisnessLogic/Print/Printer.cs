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
                if (item.Debit_Amount != 0)
                {
                    Console.WriteLine($"the debit is {item.Debit_Amount} for category {item.CategoryData}");
                }
            }

            PrintUnmappedData(mappedData);

            double totalExpenses = distinctCategoryData.Sum(x => x.Debit_Amount);
            Console.WriteLine($"\n the total expenses of the given month is {totalExpenses}");
        }

        private void PrintUnmappedData(List<ExpenseMapper> mappedData)
        {
            Console.WriteLine("\n unmapped expenses below \n");

            IEnumerable<ExpenseMapper> notMappedCollection = mappedData.Where(item => item.CategoryData.CategoryKey == "Not Mapped Yet");

            foreach (ExpenseMapper unMapItem in notMappedCollection)
            {
                if (unMapItem.Transaction_Date != null)
                    Console.WriteLine($"the debit is {unMapItem.Price_Amount} and the transaction date was on {unMapItem.Transaction_Date} for the category {unMapItem.CategoryData}");
            }

            Console.WriteLine($"\n total expense of unmapped data is {notMappedCollection.Sum(x => x.Debit_Amount)}");

            Console.WriteLine("\n foreigen currency expenses bellow : \n");
            IEnumerable<ExpenseMapper> foreignCurrCollection = mappedData.Where(item => item.CategoryData.CategoryKey == "ForeignerCurrency");

            foreach (ExpenseMapper foreignCurrItem in foreignCurrCollection)
            {
                if (foreignCurrItem.Transaction_Date != null)
                    Console.WriteLine($"the debit is {foreignCurrItem.Price_Amount} and the transaction date was on {foreignCurrItem.Transaction_Date} for the category {foreignCurrItem.CategoryData}");
            }

        }
    }
}
