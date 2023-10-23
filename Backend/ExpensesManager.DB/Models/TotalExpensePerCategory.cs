using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DB.Models
{
    public class TotalExpensePerCategory
    {
        [Key]
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public double Total_Amount { get; set; }
        public string Category { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
