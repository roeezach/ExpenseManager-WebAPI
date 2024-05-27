using ExpensesManager.Services;
using ExpensesManager.Services.Map.Common;
using System.Data;

namespace ExpensesManager.Services.BuisnessLogic.Map.ExpenseMappers
{
    public class HapoalimExpenseMapper : ExpenseMapper
    {
        #region Properties

        [DataNames("CardInfo")]
        public string Card_Details { get; set; }

        [DataNames("TransDate")]
        public DateTime? Transaction_Date { get; set; }

        [DataNames("ChargedMonth")]
        public DateTime? ChargedMonth { get; set; }

        [DataNames("Desc")]
        public string Expense_Description { get; set; }

        [DataNames("Price")]
        public double Price_Amount { get; set; }

        [DataNames("Debit")]
        public double Debit_Amount { get; set; }

        [DataNames("TransactionID")]
        public int TransactionID { get; set; }

        [DataNames("DiscountAmount")]
        public double DiscountAmount { get; set; }

        [DataNames("DiscountPrecentage")]
        public double DiscountPrecentage { get; set; }

        [DataNames("AttachedToIndex")]
        public string AttachedToIndex { get; set; }

        [DataNames("BaseRate")]
        public string BaseRate { get; set; }


        [DataNames("BaseRateForCharging")]
        public double BaseRateForCharging { get; set; }

        [DataNames("TransactionType")]
        public string TransactionType { get; set; }


        [DataNames("OtherDetails")]
        public string AdditionalDetails { get; set; }

        #endregion

        public DataTable CustomNamingColumns(DataTable dataTable)
        {
            dataTable.Columns["Column0"].ColumnName = "CardInfo";
            dataTable.Columns["Column1"].ColumnName = "ChargedMonth";
            dataTable.Columns["Column2"].ColumnName = "TransDate";
            dataTable.Columns["Column3"].ColumnName = "Desc";
            dataTable.Columns["Column4"].ColumnName = "Price";
            dataTable.Columns["Column5"].ColumnName = "Debit";
            dataTable.Columns["Column6"].ColumnName = "TransactionID";
            dataTable.Columns["Column12"].ColumnName = "TransactionType";

            return dataTable;
        }
    }
}
