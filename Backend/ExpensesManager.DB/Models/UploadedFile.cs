namespace ExpensesManager.DB;
using System.ComponentModel.DataAnnotations;

public class UploadedFile
{
    [Key]
    public int Id { get; set; }
    public int UserID { get; set; }
    public string FileName { get; set; }
    public DateTime UploadDate { get; set; }
}
