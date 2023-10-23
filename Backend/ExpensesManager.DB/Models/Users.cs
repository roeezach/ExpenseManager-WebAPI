using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesManager.DB.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [Column("Username", Order = 2)]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string SplitwisePassword { get; set; }
        public string SW_User_ID { get; set; }
    }
}
