using ExpensesManager.Services;
using ExpensesManager.Services.Map.Common;
using System.Data;

namespace ExpensesManger.Services.BuisnessLogic.Map.ExpenseMappers
{
    internal class HabinleuimiExpenseMapper : ExpenseMapper
    {
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

        
        public DataTable CustomNamingColumns(DataTable dataTable)
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

            return dataTable;

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

    }
}
