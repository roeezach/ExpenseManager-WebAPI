using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesManager.DB.Models
{
    public class SwRecords
    {
        [Key]
        [Required]
        public int Internal_TransactionID { get; set; }
        [Required]
        public long SW_TransactionID { get; set; }
        [Required]
        [ForeignKey("SplitwiseUserID")]
        public int SW_User_ID { get; set; }
        [Required]
        public double Total_Cost { get; set; }
        public string? Creation_Method { get; set; }
        [Required]
        public double Owed_Share { get; set; }
        [Required]
        public double Paid_Share { get; set; }
        public string Expense_Creation_Date{ get; set; }
        public string Record_Creation_Date { get; set; }
        public string Expense_Description { get; set; }
        [Required]        
        public string Linked_Month { get; set; }
        [Required]
        public string Linked_Year { get; set; }
        [Required]
        public string Category { get; set; }
    }
}