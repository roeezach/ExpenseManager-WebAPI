using ExpensesManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.BuisnessLogic.Map.Common;
using ExpensesManger.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ExpensesManager.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MapperController : ControllerBase
    {
        private readonly IExpenseMapperService m_ExpenseMapperService;
        private readonly IExpenseReaderService m_ExpenseReaderService;

        public MapperController(IExpenseMapperService expenseMapperService, IExpenseReaderService expenseReaderService, IHttpContextAccessor httpContextAccessor)
        {
            this.m_ExpenseMapperService = expenseMapperService;
            this.m_ExpenseReaderService = expenseReaderService;
        }

        [HttpGet(Name = "GetMappedExpenses")]
        public IActionResult GetMappedExpenses()
        {
            return Ok(m_ExpenseMapperService.GetMapExpenses());
        }

        [HttpGet(Name = "GetMappedExpensesPerMonth")]
        public IActionResult GetMappedExpensesPerMonth(int Month, int Year, int userId)
        {
            return Ok(m_ExpenseMapperService.GetMapExpensesPerMonth(Month, Year, userId));
        }

        [HttpPost]
        public IActionResult CreateMappedExpenses(string fileName, BankTypes.FileTypes fileType,int userID, DateTime chargedDate)
        {
            DataTable fileData = m_ExpenseReaderService.GetReadFile(m_ExpenseReaderService.GetPathWithFile(fileName));
            var mappedExpense = m_ExpenseMapperService.CreateExpenses(fileData, fileType, userID, chargedDate );

            int routeMonth = chargedDate.Month - 1;
            int routeYear = chargedDate.Year;

            if (routeMonth == 1)
            {
                routeMonth = 12; 
                routeYear--; 
            }
            else
            {
                routeMonth--;
            }

            return CreatedAtRoute("GetMappedExpensesPerMonth", new { Month = routeMonth, Year = routeYear, userId = userID }, mappedExpense);
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
