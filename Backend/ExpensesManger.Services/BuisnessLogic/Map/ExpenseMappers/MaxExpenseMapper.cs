using ExpensesManager.Services;
using ExpensesManager.Services.Map.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesManger.Services.BuisnessLogic.Map.ExpenseMappers
{
    public class MaxExpenseMapper : ExpenseMapper
    {

        [DataNames("TransDate")]
        public DateTime? Transaction_Date { get; set; }
        
        [DataNames("Desc")]
        public string? Expense_Description { get; set; }

        [DataNames("CardInfo")]
        public string? Card_Details { get; set; }

        [DataNames("TransactionType")]
        public string? TransactionType { get; set; }

        [DataNames("Debit")]
        public double Debit_Amount { get; set; }

        [DataNames("Price")]
        public double Price_Amount { get; set; }

        [DataNames("ChargedMonth")]
        public DateTime? ChargedMonth { get; set; }

        [DataNames("OtherDetails")]
        public string? AdditionalDetails { get; set; }

        public DataTable CustomNamingColumns(DataTable dataTable)
        {
            dataTable.Columns[0].ColumnName = "TransDate";
            dataTable.Columns[1].ColumnName = "Desc";
            dataTable.Columns[3].ColumnName = "CardInfo";
            dataTable.Columns[4].ColumnName = "TransactionType"; 
            dataTable.Columns[5].ColumnName = "Debit";
            dataTable.Columns[7].ColumnName = "Price";
            dataTable.Columns[9].ColumnName = "ChargedMonth";
            dataTable.Columns[14].ColumnName = "OtherDetails";

            return dataTable;
        }
    }
}
