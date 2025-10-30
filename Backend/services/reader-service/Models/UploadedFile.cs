using System.ComponentModel.DataAnnotations;

namespace ExpensesManager.ReaderService.Models
{
    public class UploadedFile
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileType { get; set; }
        public int LinkedMonth { get; set; }
        public int LinkedYear { get; set; }
    }
}
