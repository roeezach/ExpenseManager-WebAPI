using ExpensesManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.BuisnessLogic.Map.Common;
using ExpensesManger.Services.Contracts;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MapperController : ControllerBase
    {
        private readonly IExpenseMapperService m_ExpenseMapperService;
        private readonly IExpenseReaderService m_ExpenseReaderService;

        public MapperController(IExpenseMapperService expenseMapperService, IExpenseReaderService expenseReaderService)
        {
            this.m_ExpenseMapperService = expenseMapperService;
            this.m_ExpenseReaderService = expenseReaderService;
        }

        [HttpGet(Name = "GetMappedExpenses")]
        public IActionResult GetMappedExpenses()
        {
            return Ok(m_ExpenseMapperService.GetMapExpenses());
        }

        [HttpPost]
        public IActionResult CreateMappedExpenses(string fileName, BankTypes.FileTypes fileType,int userID, DateTime chargedDate)
        {
            DataTable fileData = m_ExpenseReaderService.GetReadFile(m_ExpenseReaderService.GetPathWithFile(fileName));
            var mappedExpense = m_ExpenseMapperService.CreateExpenses(fileData, fileType, userID, chargedDate );

            return CreatedAtRoute("GetMappedExpenses" , new { createdFileName = fileName } , mappedExpense);
        }

        [HttpDelete]
        public IActionResult DeleteMappedExpenses(int linkedMonth,int linkedYear, int userID)
        {            
            m_ExpenseMapperService.DeleteExpenses(linkedMonth, linkedYear, userID);
            return Ok();
        }

        [HttpPut]
        public IActionResult EditMappedExpenses(ExpenseRecord editedExpense, int expenseID)
        {
            return Ok(m_ExpenseMapperService.EditExpense(editedExpense, expenseID));
        }

    }
}
