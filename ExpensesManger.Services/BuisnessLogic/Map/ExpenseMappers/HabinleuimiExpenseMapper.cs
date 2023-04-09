using ExpensesManager.Services;
using ExpensesManager.Services.Map.Common;
using System.Data;

namespace ExpensesManger.Services.BuisnessLogic.Map.ExpenseMappers
{
    internal class HabinleuimiExpenseMapper : ExpenseMapper
    {
 
        [DataNames("CardInfo")]
        public string CardDetails { get; set; }
        
        [DataNames("TransDate")]
        public DateTime? Transaction_Date { get; set; }
        
        [DataNames("Desc")]
        public string ExpenseDescription { get; set; }
        
        [DataNames("Price")]
        public double PriceAmount { get; set; }
        
        [DataNames("Debit")]
        public double DebitAmount { get; set; }
        
        [DataNames("ForegnierDebitAmount")]
        public string ForegnierDebitAmount { get; set; }
        
        [DataNames("DebitCurrency")]
        public string DebitCurrency { get; set; }

        [DataNames("PriceCurrency")]
        public string PriceCurrency { get; set; }


        [DataNames("ExchangeDescription")]
        public string ExchangeDescription { get; set; }
        
        public double ExchangeRate { get; set; }

        public double ExchangeCommission { get; set; }


        public DataTable CustomNamingColumns(DataTable dataTable)
        {
            dataTable.Columns["Column0"].ColumnName = "CardInfo";
            dataTable.Columns["Column1"].ColumnName = "TransDate";
            dataTable.Columns["Column2"].ColumnName = "Desc";
            dataTable.Columns["Column3"].ColumnName = "Price";
            dataTable.Columns["Column4"].ColumnName = "Debit";
            dataTable.Columns["Column5"].ColumnName = "ForegnierDebitAmount";

            if (ContainColumn(dataTable, "Column6"))
            {
                dataTable.Columns["Column6"].ColumnName = "DebitCurrency";
                
                if (ContainColumn(dataTable, "Column7"))
                    dataTable.Columns["Column7"].ColumnName = "ExchangeDescription";
            }

            return dataTable;
        }

        private bool ContainColumn(DataTable table, string columnName)
        {
            DataColumnCollection columns = table.Columns;
            if (columns != null && columns.Contains(columnName))
            {
                return true;
            }

            return false;
        }
    }
}
