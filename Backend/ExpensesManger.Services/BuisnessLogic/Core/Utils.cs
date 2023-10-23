using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ExpensesManager.BuisnessLogic.Core
{
    public static class Utils
    {
        #region Const

        private const int ONE_MONTH = 1;
        private const int ONE_YEAR = 1;
        private const int MIN_INVALID_MONTH = 0;
        private const int MAX_INVALID_MONTH = 13;
        private const int LAST_MONTH_IN_YEAR = 12;
        private const int FIRST_MONTH_IN_YEAR = 1;
        private const int DEFAULT_DAY = 1;
        private const int REGULAR_CHARGE_DAY = 10;
        private const int SPECIAL_CHARGE_DAY = 1;

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
                int month = GetExpenseLinkedMonth(parsedDate, currentPeriodRange);

                return GetExpenseLinkedYearAsDateTime(currentPeriodRange, creditCardChargeDay, month);
            }
            else
            {
                return new DateTime(currentPeriodRange.Year, currentPeriodRange.Month, DEFAULT_DAY);
            }
        }
        public static double GetExchangeRate(string input)
        {
            string pattern = @"(?<=\bבשער\s)[\d,]+(?:\.\d+)?(?=\s*\$|\s*€|\s*£)";

            Match match = Regex.Match(input, pattern);

            if (match.Success && double.TryParse(match.Value.Replace(",", ""), out double exchangeRate))
            {
                return exchangeRate;
            }

            return 0;
        }


        public static int GetExpenseLinkedMonth(DateTime expenseDate, DateTime ChargedDate)
        {
            int returnedMonth;
            if(ChargedDate.Day == REGULAR_CHARGE_DAY)
            {
                returnedMonth = ShouldKeepLinkedMonth(expenseDate, ChargedDate.Day, ChargedDate.Month);
                return ValidMonth(returnedMonth);
            }
            else if(ChargedDate.Day == SPECIAL_CHARGE_DAY)
            {
                returnedMonth = ChargedDate.Month - ONE_MONTH;
                return ValidMonth(returnedMonth);
            }
            else
            {
                return expenseDate.Month;
            }
        }

        public static int ValidMonth(int month)
        {
            if (month == MIN_INVALID_MONTH)
                return LAST_MONTH_IN_YEAR;
            else if (month == MAX_INVALID_MONTH)
                return FIRST_MONTH_IN_YEAR;
            else
                return month;
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
            ///TODO - remove when we finish developing user settings.
            if (userID == 344511326)
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

        public static string ReformatHebrewString(string originString)
        {
            if (!string.IsNullOrEmpty(originString))
            {
                
                string hebrewRegex = @"-ל?\s*([\u0590-\u05FF]+)?\s*";
                Match hebrewMatch = Regex.Match(originString, hebrewRegex);
                string hebrewLetters = "";
                if (hebrewMatch.Success)
                {
                    hebrewLetters = new string(hebrewMatch.Groups[1].Value.Reverse().ToArray());
                }

                // Regex to extract the currency symbol
                string currencyRegex = @"\$\s*";
                Match currencyMatch = Regex.Match(originString, currencyRegex);
                string currency = "";
                if (currencyMatch.Success)
                {
                    currency = currencyMatch.Value.Trim();
                }

                // Regex to extract the number
                string numberRegex = @"\d+(\.\d+)?";
                Match numberMatch = Regex.Match(originString, numberRegex);
                string number = "";
                if (numberMatch.Success)
                {
                    number = numberMatch.Value;
                }

                // Regex to extract the date
                string dateRegex = @"\d{1,2}/\d{1,2}/\d{2}";
                Match dateMatch = Regex.Match(originString, dateRegex);
                string date = "";
                if (dateMatch.Success)
                {
                    date = dateMatch.Value;
                }

                string reformattedString = $" {hebrewLetters + " ל"} {currency}{number} {"ב  " + date}";


                return reformattedString;
            }
            else
                return originString;
        }

        #endregion

        #region Private Methods

        private static int ShouldKeepLinkedMonth(DateTime expenseDate, int creditCardChargeDay, int creditCardChargeMonth)
        {
            if (creditCardChargeDay <= expenseDate.Day && creditCardChargeMonth > expenseDate.Month)
                return expenseDate.Month;
            else if (creditCardChargeDay > expenseDate.Day && creditCardChargeMonth == expenseDate.Month)
                return expenseDate.Month - ONE_MONTH;
            else
                return expenseDate.Month;
        }

        #endregion
    }
}
