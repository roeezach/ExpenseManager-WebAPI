using System.Data;
using ExpensesManager.DB;

namespace ExpensesManger.Services.Contracts
{
    public interface IExpenseReaderService
    {
        DataTable GetReadFile(string path);
        DateTime GetExpenseFileDateRangeStart();
        string CreatePathForFiles();
        bool SaveUploadedFile(string fileName, DateTime uploadDate, int userID, string fileType, int monthToMap, int yearToMap);
        string GetPathWithFile(string fileName);
        public string GetDefaultFilePath();
        string EditPathWithFiles(string newPath, string fileName);
        void DeletePathWithoutFiles();
        List<UploadedFile> GetUploadedFiles(int userID);
    }
}
