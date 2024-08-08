using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.DB;
using ExpensesManager.Services;
using ExpensesManager.Services.Contracts;
using System.Data;

namespace ExpensesManager.Services.Services
{
    public class ExpenseReadService : IExpenseReaderService
    {
        private const string FILES_FOLDER_PREFIX = $"ExpenseFileByMonth";
        private readonly AppDbContext appDbContext;
        public ExpenseReader Reader { get; set; }
        public ExpenseReadService(AppDbContext context)
        {
            appDbContext = context;
            Reader = new ExpenseReader();
        }

        public string CreatePathForFiles()
        {
            string filePath = GetDefaultFilePath();
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            return filePath;
        }

        public void DeletePathWithoutFiles()
        {
            Directory.Delete(GetDefaultFilePath());
        }

        public string EditPathWithFiles(string newPath, string fileName)
        {
            string currentPath = GetPathWithFile(fileName);
            string? editedPath = currentPath.Replace(currentPath, newPath);
            return string.Join(editedPath, $"{fileName}.xls");
        }

        public string GetPathWithFile(string fileName)
        {
            string filePath = GetDefaultFilePath();
            return Path.Join(filePath, $"{fileName}.xls");
        }

        public string GetDefaultFilePath()
        {
            Environment.SpecialFolder folder = Environment.SpecialFolder.ApplicationData;
            string? folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, FILES_FOLDER_PREFIX);
            return filePath;
        }

        public DateTime GetExpenseFileDateRangeStart()
        {
            return Reader.GetExpenseFileDateRangeStart();
        }

        public DataTable GetReadFile(string path)
        {
            return Reader.ReadFile(path);
        }

        public bool SaveUploadedFile(string fileName, DateTime uploadDate, int userID, string fileType, int monthToMap, int yearToMap)
        {
            if (!appDbContext.UploadedFiles.Any(file => file.FileName == fileName))
            {
                var uploadedFile = new UploadedFile
                {
                    Id = Utils.GenerateRandomID(),
                    FileName = fileName,
                    UploadDate = uploadDate,
                    UserID = userID,
                    FileType = fileType,
                    LinkedMonth = monthToMap,
                    LinkedYear = yearToMap
                };
                appDbContext.UploadedFiles.Add(uploadedFile);
                appDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<UploadedFile> GetUploadedFiles(int userID)
        {
            return appDbContext.UploadedFiles.Where(file => file.UserID == userID).ToList();
        }
    }
}
