using System.Data;
using ExpensesManager.BuisnessLogic.Core;
using System.Reflection;
using Newtonsoft.Json;
using ExpensesManager.Services.Map.Models;
using ExpensesManager.Services.Map.Common;
using ExpensesManger.Services.BuisnessLogic.Map;
using ExpensesManager.DB.Models;

namespace ExpensesManager.Services
{
    public class ExpenseMapper
    {
        #region Constants
        private const string MONTHLY_PAYMENT_STRING = "מ - ";
        #endregion

        #region Enum
        
        public enum FileTypes
        {
            Expenses,
            Movements
        }

        #endregion Enum

        #region Properties
        [DataNames("CardInfo")]
        public string Card_Details { get; set; }
        [DataNames("TransDate")]
        public DateTime? Transaction_Date { get; set; }
        [DataNames("Desc")]
        public string Expense_Description { get; set; }
        [DataNames("Price")]
        public double Price_Amount { get; set; }
        [DataNames("Debit")]
        public double Debit_Amount { get; set; }
        [DataNames("OtherDetails")]
        public string AdditionalDetails { get; set; }
        [DataNames("Curr")]
        public string Currency { get; set; }
        public double Exchange_Rate { get; set; }
        [DataNames("ExchangeDescription")]
        public string Exchange_Description { get; set; }

        public double ExchangeCommission { get; set; }

        public CategoryExpenseMapper CategoryData{ get; set; }
        //total amount of the card should not be in this layer
        #endregion Properties

        #region Public Methods
        /// <summary>
        /// the methods mapping the file into the constant categories
        /// </summary>
        /// <param name="dataTable">the raw data from the file</param>
        /// <param name="fileType"> expenses, movment or other </param>
        /// <returns>List with mapped categories </returns>
        public List<ExpenseMapper> MapFile(DataTable dataTable,FileTypes fileType)
        {
            NamingColumns(dataTable,fileType);
            DataNamesMapper<ExpenseMapper> namesMapper = new DataNamesMapper<ExpenseMapper>();

            List<ExpenseMapper>? mappedListWithColumnNames = namesMapper.Map(dataTable).ToList();
            List<ExpenseMapper> mappedList = new List<ExpenseMapper>();

            foreach (ExpenseMapper? mappedRow in mappedListWithColumnNames)
            {
                if (!string.IsNullOrEmpty(mappedRow.Expense_Description) && mappedRow.Expense_Description != CategoryExpenseMapper.TITLE_DESCRIPTION)
                {
                    mappedRow.CategoryData = new CategoryExpenseMapper()
                    {
                       Category = CategoryExpenseMapper.GetCategoryMapping(mappedRow.Expense_Description)
                    };
                    mappedList.Add(mappedRow);
                }
            }

            return mappedList;  
        }
       
        public List<ExpenseRecord> GetExpensesPerMonth(List<ExpenseRecord> distinctCategoryList,DateTime currentExpenseMonth, int userID)
        {
            List<ExpenseRecord> filteredList = distinctCategoryList.Where(item => item.Price_Amount > 0).ToList();

            return filteredList.Where(t => ((t.Transaction_Date == currentExpenseMonth.Month.ToString()) ||
                                               DateUtils.GetExpenseLinkedMonth(currentExpenseMonth, DateUtils.GetUserChargeDay(userID)) == currentExpenseMonth.Month)).ToList();
        }

        #endregion Public Methods
        
        #region Private Methods

        private void NamingColumns(DataTable dataTable, FileTypes fileType)
        {
            if (fileType == FileTypes.Expenses)
            {
                dataTable.Columns["Column0"].ColumnName = "CardInfo";
                dataTable.Columns["Column1"].ColumnName = "TransDate";
                dataTable.Columns["Column2"].ColumnName = "Desc";
                dataTable.Columns["Column3"].ColumnName = "Price";
                dataTable.Columns["Column4"].ColumnName = "Debit";
                dataTable.Columns["Column5"].ColumnName = "OtherDetails";
                dataTable.Columns["Column6"].ColumnName = "Curr";

                if (ContainColumn(dataTable, "Column7"))
                    dataTable.Columns["Column7"].ColumnName = "ExchangeDescription";
            }
            if (fileType == FileTypes.Movements)
            {
                dataTable.Columns["Column0"].ColumnName = "CardInfo";
                dataTable.Columns["Column1"].ColumnName = "Balance";
                dataTable.Columns["Column2"].ColumnName = "Date";
                dataTable.Columns["Column3"].ColumnName = "Credit";
                dataTable.Columns["Column4"].ColumnName = "Debit";
                dataTable.Columns["Column5"].ColumnName = "Description";
                dataTable.Columns["Column6"].ColumnName = "ID";
                dataTable.Columns["Column7"].ColumnName = "Type";
            }
        }
     
        private bool ContainColumn(DataTable table, string columnName)
        {
            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                return true;
            }

            return false;
        }
        
        #endregion
    }
}
