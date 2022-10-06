using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.DB.Models
{
    public class Categories
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string MappedCategoriesJson { get; set; }
    }
}
