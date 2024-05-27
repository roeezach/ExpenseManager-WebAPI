using Microsoft.AspNetCore.Mvc;
using ExpensesManager.Services.Contracts;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReaderController : ControllerBase
    {

        private readonly IExpenseReaderService m_ExpenseReaderService;

        public ReaderController(IExpenseReaderService expenseReaderService)
        {
            m_ExpenseReaderService = expenseReaderService;
        }

        [HttpGet(Name = "GetFilePath")]
        public IActionResult GetFilePath()
        {
            return Ok(m_ExpenseReaderService.GetDefaultFilePath());
        }

        [HttpGet(Name = "GetUploadedFiles")]
        public IActionResult GetUploadedFiles(int userID)
        {
            return Ok(m_ExpenseReaderService.GetUploadedFiles(userID));
        }

        [HttpPost]
        public IActionResult CreateFilesPath(IFormFile file, int userID, string fileType, int monthToMap, int yearToMap)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string path = m_ExpenseReaderService.CreatePathForFiles();
            string filePath = Path.Combine(path, $"{fileName}{extension}");
            if (System.IO.File.Exists(filePath))
            {
                return Conflict("File already exists.");
            }
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            bool isFileCreated = m_ExpenseReaderService.SaveUploadedFile(fileName, System.DateTime.Now, userID, fileType, monthToMap, yearToMap);
            return CreatedAtRoute("GetFilePath", new { filePath = path, fileName, isFileCreated });
        }

        [HttpDelete]
        public IActionResult DeleteFilePath()
        {

            m_ExpenseReaderService.DeletePathWithoutFiles();
            return Ok();
        }

        [HttpPut]
        public IActionResult EditFilePath(string newFilePath, string fileName)
        {
            return Ok(m_ExpenseReaderService.EditPathWithFiles(newFilePath, fileName));
        }

        /// <summary>
        /// this method assume that on the full path there are expenses file to read
        /// </summary>
        /// <param name="pathWithFiles"></param>
        /// <returns>data table of the file after being read </returns>
        [HttpGet("{fileName}", Name = "GetReadFile")]
        internal IActionResult GetReadFile(string fileName)
        {
            string pathWithFiles = m_ExpenseReaderService.GetPathWithFile(fileName);
            return Ok(m_ExpenseReaderService.GetReadFile(pathWithFiles));
        }

        [HttpGet(Name = "GetExpenseFileDateRangeStart")]
        public IActionResult GetExpenseFileDateRangeStart()
        {
            return Ok(m_ExpenseReaderService.GetExpenseFileDateRangeStart());
        }
    }
}
