using ExpensesManager.Services;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.Integrations.GetExpensesAPI
{
    internal static class GetExpensesExecuter
    {
        private const int MONTHS_RANGE = 1;


        internal static DateTime ExpensesDatesStart { get; set; }

        internal static Task<string> SplitWiseApiHandlerWithDates(DateTime dateRangeStart, IConfiguration config)
        {
            ExpensesDatesStart = dateRangeStart;
            GetExpensesRequestBuilder.HeaderBuilder();
            GetExpensesRequestBuilder.SplitwiseAuthenticationRequestBuilder(config);

            GetExpensesRequestBuilder.SetDateRangeStartEndDateRequest(ExpensesDatesStart, ExpensesDatesStart.AddMonths(MONTHS_RANGE));
            Task<string> expensesDataString = GetExpensesResponseProcesser.GetTaskAsyncResponse();

            return expensesDataString;
        }

        internal static Task<string> SplitWiseApiHandler(DateTime dateRangeStart, IConfiguration config)
        {
            ExpensesDatesStart = dateRangeStart;
            GetExpensesRequestBuilder.HeaderBuilder();
            GetExpensesRequestBuilder.SplitwiseAuthenticationRequestBuilder(config);

            Task<string> expensesDataString = GetExpensesResponseProcesser.GetTaskAsyncResponse();

            return expensesDataString;
        }

    }
}
