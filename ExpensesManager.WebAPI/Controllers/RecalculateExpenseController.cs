using ExpensesManager.DB.Models;
using ExpensesManger.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RecalculateExpenseController : ControllerBase
    {
        private readonly IRecalculatedExpenseService m_RecalculatedExpenseService;

        public RecalculateExpenseController(IRecalculatedExpenseService recalculatedExpenseService)
        {
            m_RecalculatedExpenseService = recalculatedExpenseService;
        }

        [HttpGet(Name = "GetAllRecalculatedExpenseRecords")]
        public IActionResult GetAllRecalculatedExpenseRecords()
        {
            return Ok(m_RecalculatedExpenseService.GetAllRecalculatedExpenseRecords());
        }

        [HttpGet(Name = "GetRecalculatedExpenseRecords")]
        public IActionResult GetRecalculatedExpenseRecords(DateTime fromDate)
        {
            return Ok(m_RecalculatedExpenseService.GetRecalculatedExpenseRecords(fromDate));
        }

        [HttpPut(Name = "EditRecalculatedExpenseRecord")]
        public IActionResult EditRecalculatedExpenseRecord(RecalculatedExpenseRecord record)
        {
            return Ok(m_RecalculatedExpenseService.EditRecalculatedExpenseRecord(record));
        }

        [HttpPost(Name = "CreateRecalculatedExpenseRecords")]
        public IActionResult CreateRecalculatedExpenseRecords(DateTime fromDate)
        {
            List<RecalculatedExpenseRecord> createdRecords = m_RecalculatedExpenseService.CreateRecalculatedExpenseRecords(fromDate);

            return CreatedAtRoute("GetRecalculatedExpenseRecords", new { recalculatedTimeStamp = fromDate }, createdRecords);
        }

        [HttpDelete(Name = "DeleteRecalculatedExpenseRecords")]
        public IActionResult DeleteRecalculatedExpenseRecords(DateTime fromDate)
        {
            m_RecalculatedExpenseService.DeleteRecalculatedExpenseRecords(fromDate);
            return Ok();
        }

    }
}
