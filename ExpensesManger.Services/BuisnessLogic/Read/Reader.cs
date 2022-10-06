using ExcelDataReader;
using System.Data;
using System.Text;

namespace ExpensesManager.Services
{
    public class ExpenseReader 
    {
        private const int SINGLE_TABLE_LOCATION = 0; 
      
        public DataTable ReadFile(string path)
        {
            DataTable dataTable = ReadExcelFile(path);

            return dataTable;
        }

        internal DateTime GetExpenseFileDateRangeStart()
        {
            DateTime expenseDateTime = DateTime.MinValue;
            bool isDateTimeValid = false;
            Console.WriteLine("please enter the expense file start date range as follows: yyyy/MM/DD \n\nnote that this app is calculated the expenses based on one month period");
            
            while(!isDateTimeValid)
            {
                if (DateTime.TryParse(Console.ReadLine(), out expenseDateTime))
                    isDateTimeValid = true;
                else
                    Console.WriteLine("\n invalid date, try again");               
            }

            return expenseDateTime;
        }

        private DataTable ReadExcelFile(string filePath)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using (stream)
            {
                using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true,                        
                    }
                });

                DataTableCollection table = result.Tables;
                DataTable resultTable = table[SINGLE_TABLE_LOCATION];
                return resultTable;
            }
        }
    }
}
