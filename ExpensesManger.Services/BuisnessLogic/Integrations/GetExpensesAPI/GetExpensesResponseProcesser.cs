
using ExpensesManager.Services;

namespace ExpensesManager.Integrations.GetExpensesAPI
{
    internal static class GetExpensesResponseProcesser
    {
        internal const int  LOGGED_IN_USER_ID = 111; //for real cases - actual user id and will not be const

        internal static async Task<string> GetTaskAsyncResponse()
        {
            HttpClient currentClient = GetExpensesRequestBuilder.apiClient;
            using HttpResponseMessage response = await currentClient.GetAsync(currentClient.BaseAddress.AbsoluteUri.ToString());
            if (response.IsSuccessStatusCode)
            {
                string expensesData = await response.Content.ReadAsStringAsync();
                return expensesData;
            }
            else
                throw new  HttpRequestException(response.ReasonPhrase);
        }

        /// <summary>
        /// prequisite the SW expenses are saved to DB and we can calculate the total debit amout per category
        /// </summary>
        /// <param name="mappedDataExpenses"></param>
        /// <param name="mapper"></param>
        /// <param name="expensesDateTime"></param>
        /// <returns>correct value of total amount after relaying on SW API </returns>
        internal static List<ExpenseMapper> RecalculateDebitAmount(List<ExpenseMapper> mappedDataExpenses, DateTime expensesDateTime)
        {
            throw new NotImplementedException();
            //List<SwRecordsDO> swRecords = SqliteDataAccess.GetDO<SwRecordsDO>(SqliteDataAccess.TABLE_NAME_SPLITWISE);
            //IEnumerable<SwRecordsDO> currMonthRecord = swRecords.Where(swExpense => (Convert.ToInt32(swExpense.Linked_Month) == expensesDateTime.Month) &&
            //                                                            swExpense.SW_User_ID == LOGGED_IN_USER_ID);
            
            //foreach (SwRecordsDO recordsDO in currMonthRecord)
            //{
            //    if(recordsDO.Paid_Share > 0)                
            //        mappedDataExpenses.Find(c => c.Category.ToString() == recordsDO.Category).Debit_Amount -= Convert.ToDouble(recordsDO.Owed_Share);                
            //    else
            //    {
            //        mappedDataExpenses.Find(c => c.Category.ToString() == recordsDO.Category).Debit_Amount += Convert.ToDouble(recordsDO.Owed_Share);
            //        mappedDataExpenses.Find(c => c.Category.ToString() == recordsDO.Category).ExpensesInCategory.Add(CreateNewMapperItemBasedOnSwRecord(recordsDO));
            //    }
            //}                   
            //return mappedDataExpenses;
        }

        private static ExpenseMapper CreateNewMapperItemBasedOnSwRecord(/*SwRecordsDO recordsDO*/)
        {
            throw new NotImplementedException();

            //return new Mapper()
            //{
            //    Transaction_Date = DateTime.Parse(recordsDO.Expense_Creation_Date),
            //    Debit_Amount = recordsDO.Owed_Share,
            //    Exchange_Description = recordsDO.Expense_Description,
            //    Price_Amount = recordsDO.Total_Cost,
            //    Category = Enum.Parse<Mapper.MappedCategory>(recordsDO.Category)
            //};
        }
    }
}
