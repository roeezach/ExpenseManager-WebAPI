using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DB.Models
{
    public class ExpenseRecord
    {
        [Key]
        public int TransactionID { get; set; }
        public string? Card_Details { get; set; }
        public string? Transaction_Date { get; set; }
        public string? Record_Create_Date { get; set; }
        [Required]
        public string Expense_Description { get; set; }
        public double? Price_Amount { get; set; }
        [Required]
        public double Debit_Amount { get; set; }
        public string? Currency { get; set; }
        public double Exchange_Rate { get; set; }
        public string? Exchange_Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int User_ID { get; set; }
        public string Linked_Month { get; set; }
    }
}
