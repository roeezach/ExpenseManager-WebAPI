using System;
using System.Text.RegularExpressions;

namespace ExpensesManager.BuisnessLogic.Core
{
    public static class DateUtils
    {
        #region Const

        private const int ONE_MONTH = 1;
        private const int ONE_YEAR = 1;
        private const int INVALID_MONTH = 0;
        private const int LAST_MONTH_IN_YEAR = 12;
        private const int FIRST_MONTH_IN_YEAR = 1;
        private const int DEFAULT_DAY = 1;

        #endregion

        #region Methods

        /// <summary>
        /// search for the price amount of online shopping , the bigger number is 999.9, on AliExpress the bank is not providing exchange rate info
        /// </summary>
        /// <param name="item"></param>
        public static double RegexMatcherForgeignCurrency(string searchItem)
        {
            Match numStructureFinder;
            if (Regex.IsMatch(searchItem, @"/\d\.\d\d/m|/\d\d\.\d\d/m|/\d\d\d\.\d/m"))
            {
                numStructureFinder = Regex.Match(searchItem, @"/\d\.\d\d/m|/\d\d\.\d\d/m|/\d\d\d\.\d/m");
                double foundValue = Convert.ToDouble(numStructureFinder.Value);
                return foundValue;
            }
            else
                return 0.0d;
        }
        /// <summary>
        /// search for date pattern in a string and return the relevant month period linked to the expense
        /// </summary>
        /// <param name="searchItem">string to search in </param>
        /// <param name="creditCardChargeDay">charge day of the credit card. i.e - 1, 10 </param>
        /// <param name="year">year of the expense</param>
        /// <returns></returns>
        public static DateTime RegexMatcherDateToCharge(string searchItem, int creditCardChargeDay, DateTime currentPeriodRange)
        {
            Match dateStructureFinder;
            string pattern = @"\d\d.\d\d|\d\.\d\d|\d\.\d\.\d|\d\d.\d|\d\.\d";
            if (Regex.IsMatch(searchItem, pattern))
            {
                dateStructureFinder = Regex.Match(searchItem, pattern);
                string date = dateStructureFinder.Value.ToString() + $".{currentPeriodRange.Year}";
                DateTime parsedDate = DateTime.Parse(date);
                int month = GetExpenseLinkedMonth(parsedDate, creditCardChargeDay);

                return GetExpenseLinkedYearAsDateTime(currentPeriodRange, creditCardChargeDay, month);
            }
            else
            {
                return new DateTime(currentPeriodRange.Year, currentPeriodRange.Month, DEFAULT_DAY);
            }
        }

        public static int GetExpenseLinkedMonth(DateTime expenseDate, int creditCardChargeDay)
        {
            var returnedMonth = creditCardChargeDay >= expenseDate.Day ? expenseDate.Month - ONE_MONTH : expenseDate.Month;
            return returnedMonth == INVALID_MONTH ? LAST_MONTH_IN_YEAR : returnedMonth;
        }

        public static DateTime GetExpenseLinkedYearAsDateTime(DateTime expenseDate, int creditCardChargeDay, int parsedMonth)
        {
            if (expenseDate.Month == FIRST_MONTH_IN_YEAR && parsedMonth == LAST_MONTH_IN_YEAR)
                return new DateTime(expenseDate.Year - ONE_YEAR, parsedMonth, DEFAULT_DAY);
            else if (parsedMonth != expenseDate.Month)
                return new DateTime(expenseDate.Year, parsedMonth, DEFAULT_DAY);
            else
                return new DateTime(expenseDate.Year, expenseDate.Month, DEFAULT_DAY);
        }

        /// <summary>
        /// temportay fix to retrive user charge day, till login functionality will be implemnted
        /// TODO : Move logic to be stored in DB in users table
        /// </summary>
        public static int GetUserChargeDay(int userID)
        {
            if (userID == 19773792)
                return 1;
            else
                return 10;
        }

        /// <summary>
        /// ID generator
        /// </summary>
        /// <returns>unique random ID</returns>
        public static int GenerateRandomID()
        {
            Random random = new Random();
            return random.Next();
        }
        #endregion
    }
}
