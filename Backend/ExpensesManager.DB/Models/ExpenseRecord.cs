using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesManager.DB.Models
{
    public class ExpenseRecord
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        [Column("User_ID", Order = 2)]
        public int User_ID { get; set; }
    
        [Required]
        [Column("Expense_Description", Order = 3)]
        public string Expense_Description { get; set; }

        [Required]
        [Column("Category", Order = 4)]
        public string Category { get; set; }


        [Column("Price_Amount", Order = 5)]
        public double? Price_Amount { get; set; }

        [Column("PriceCurrency", Order = 6)]
        public string? PriceCurrency { get; set; }

        [Required]
        [Column("Debit_Amount", Order = 7)]
        public double Debit_Amount { get; set; }
        [Column("DebitCurrency", Order = 8)]
        public string? DebitCurrency { get; set; }
        
        [Column("Exchange_Description", Order = 9)]
        public string? Exchange_Description { get; set; }
        
        [Column("Exchange_Rate", Order = 10)]
        public double Exchange_Rate { get; set; }

        [Column("Linked_Month", Order = 11)]
        public string Linked_Month { get; set; }

        [Column("Linked_Year", Order = 12)]
        public string Linked_Year { get; set; }

        [Column("Transaction_Date", Order = 13)]
        public string? Transaction_Date { get; set; }

        public string? Card_Details { get; set; }

        public string? Record_Create_Date { get; set; }

    }
}
