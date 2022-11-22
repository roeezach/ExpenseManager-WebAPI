using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DB.Models
{
    public class SwRecords
    {
        [Key]
        [Required]
        public int Internal_TransactionID { get; set; }
        [Required]
        public int SW_TransactionID { get; set; }
        [Required]
        public int SW_User_ID { get; set; }
        [Required]
        public double Total_Cost { get; set; }
        public string Creation_Method { get; set; }
        public double Owed_Share { get; set; }
        public double Paid_Share { get; set; }
        public string Expense_Creation_Date{ get; set; }
        public string Record_Creation_Date { get; set; }
        public string Expense_Description { get; set; }
        public string Linked_Month { get; set; }
        public string Linked_Year { get; set; }
        public string Category { get; set; }
    }
}