using System;
using System.Collections.Generic;

namespace ExpensesManager.Integrations.SplitWiseModels
{
    public class SplitwiseExpense
    {
        public long ID { get; set; }
        public int? Group_ID { get; set; }
        public int? Friendship_ID { get; set; }
        public int? Expense_Bundle_ID { get; set; }
        public string Description { get; set; }
        public bool Repeats { get; set; }
        public string Repeat_Interval { get; set; }
        public bool Email_Reminder { get; set; }
        public int? Email_Reminder_In_Advance { get; set; }
        public string Next_Repeat { get; set; }
        public int Comments_Count { get; set; }
        public bool Payment { get; set; }
        public string Creation_Method { get; set; }
        public bool Transaction_Confirmed { get; set; }
        public int? Transaction_ID { get; set; }
        public string Transaction_Status { get; set; }
        public string Cost { get; set; }
        public string Currency_Code { get; set; }
        public List<RepaymentsItems> Repayments { get; set; }
        public DateTime Date { get; set; }
        public DateTime Created_At { get; set; }
        public ChangeExpense  Created_By { get; set; }
        public DateTime Updated_At { get; set; }
        public DateTime? Deleted_At { get; set; }
        public ChangeExpense Deleted_By { get; set; }
        public CategoryItem Category { get; set; }
        public ReceiptItem Receipt { get; set; }
        public List<Users> Users { get; set; }
               
    }
}
