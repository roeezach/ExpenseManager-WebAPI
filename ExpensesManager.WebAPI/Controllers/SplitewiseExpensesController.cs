using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.Contracts;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SplitewiseExpensesController : ControllerBase
    {
        private readonly ISplitwiseExpensesService m_SplitwiseExpensesService;

        public SplitewiseExpensesController(ISplitwiseExpensesService splitwiseExpensesService)
        {
            m_SplitwiseExpensesService = splitwiseExpensesService;
        }

        [HttpGet(Name = "GetAllSwRecords")]
        public IActionResult GetAllSwRecords()
        {
            return Ok(m_SplitwiseExpensesService.GetAllSwRecords());
        }

        [HttpGet(Name = "GetSwRecords")]
        public IActionResult GetSwRecords(DateTime fromDate)
        {
            return Ok(m_SplitwiseExpensesService.GetSwRecords(fromDate));
        }

        [HttpPost]
        public IActionResult CreateSplitewiseExpenseRecords(DateTime fromDate)
        {
            List<SwRecords> swRecords = m_SplitwiseExpensesService.CreateSwRecords(fromDate);
            return CreatedAtRoute("GetSwRecords", new { SplitewiseRecords = swRecords }, swRecords);
        }

        [HttpDelete]
        public IActionResult DeleteSplitewiseExpenseRecords(DateTime fromDate)
        {
            m_SplitwiseExpensesService.DeleteSwRecords(fromDate);
            return Ok();
        }
    }
}
