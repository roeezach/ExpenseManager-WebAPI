using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DB.Models
{
    public class RecalculatedExpenseRecord
    {
        [Key]
        public int Recalculated_Expense_Record_Trans_ID { get; set; }
        
        [Required]
        public int ExpenseRecord_TransactionID { get; set; }
        
        [Required]
        public int SW_TransactionID { get; set; }
        
        [Required]
        public int SW_UserID { get; set; }
        
        [Required]
        public string Expense_Description { get; set; }        
        
        [Required]
        public string Linked_Month { get; set; }
        [Required]
        public string Linked_Year { get; set; }

        [Required]
        public string Expense_Creation_Date { get; set; }
        
        [Required]
        public string Record_Creation_Date { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public double Owed_Share { get; set; }
        [Required]
        public double Paid_Share { get; set; }
    }
}
