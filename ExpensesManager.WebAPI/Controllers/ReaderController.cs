using Microsoft.AspNetCore.Mvc;
using ExpensesManger.Services;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost]
        public IActionResult CreateFilesPath(string fileName)
        {
            string path = m_ExpenseReaderService.CreatePathForFiles();
            return CreatedAtRoute("GetFilePath", new { filePath = path });
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
