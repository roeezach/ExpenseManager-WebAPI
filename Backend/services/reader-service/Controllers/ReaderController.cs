using System.Threading.Tasks;
using ExpensesManager.Common.Messaging;
using ExpensesManager.ReaderService.Models;
using ExpensesManager.ReaderService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesManager.ReaderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReaderController : ControllerBase
    {
        private readonly ExpenseReadService _service;

        public ReaderController(ExpenseReadService service)
        {
            _service = service;
        }

        [HttpGet("health")]
        public IActionResult Health() => Ok(new { status = "ok" });

        [HttpPost("upload-metadata")]
        public async Task<IActionResult> UploadMetadata([FromBody] UploadedFile dto)
        {
            var saved = await _service.SaveUploadedFileAsync(dto);
            return Created(string.Empty, saved);
        }
    }
}
