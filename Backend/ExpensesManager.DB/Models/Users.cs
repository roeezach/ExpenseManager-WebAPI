using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DB.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string SplitwisePassword { get; set; }
    }
}
