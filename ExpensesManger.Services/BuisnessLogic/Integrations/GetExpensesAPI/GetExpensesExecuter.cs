using ExpensesManager.Services;

namespace ExpensesManager.Integrations.GetExpensesAPI
{
    internal static class GetExpensesExecuter
    {
        private const int MONTHS_RANGE = 1;


        internal static DateTime ExpensesDatesStart { get; set; }

        internal static Task<string> SplitWiseApiHandlerWithDates(ExpenseReader reader)
        {
            ExpensesDatesStart = reader.GetExpenseFileDateRangeStart();
            GetExpensesRequestBuilder.HeaderBuilder();
            GetExpensesRequestBuilder.SplitwiseAuthenticationRequestBuilder();

            GetExpensesRequestBuilder.SetDateRangeStartEndDateRequest(ExpensesDatesStart, ExpensesDatesStart.AddMonths(MONTHS_RANGE));
            Task<string> expensesDataString = GetExpensesResponseProcesser.GetTaskAsyncResponse();

            return expensesDataString;
        }


    }
}
