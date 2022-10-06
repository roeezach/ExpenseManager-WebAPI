using System.Data;

namespace ExpensesManger.Services
{
    public interface IExpenseReaderService
    {
        DataTable GetReadFile(string path);
        DateTime GetExpenseFileDateRangeStart();
        string CreatePathForFiles();
        string GetPathWithFile(string fileName);
        public string GetDefaultFilePath();
        string EditPathWithFiles(string newPath,string fileName);
        void DeletePathWithoutFiles();
    }
}
