using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using ExpensesManager.DB.Models;
using ExpensesManger.Services.Contracts;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TotalExpensePerCategoryController : ControllerBase
    {
        private readonly ITotalExpensesPerCategoryService m_TotalExpensesPerCategoryService;
        private readonly IExpenseMapperService m_ExpenseMapperService;

        public TotalExpensePerCategoryController(ITotalExpensesPerCategoryService totalExpensesPerCategoryService, IExpenseMapperService expenseMapperService)
        {
            m_TotalExpensesPerCategoryService = totalExpensesPerCategoryService;
            m_ExpenseMapperService = expenseMapperService;
        }

        [HttpGet("{month:int}/{year:int}",Name = "GetCategoriesSumPerTimePeriod")]
        public IActionResult GetCategoriesSumPerTimePeriod(int month, int year,int userID)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategoriesSum(month, year, userID));
        }

        [HttpGet("{category}/{month:int}/{year:int}",Name = "GetCategoriesSum")]
        public IActionResult GetCategoriesSum(int month, int year, string category, int userID)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategorySum(month, year, category, userID));
        }

        [HttpGet(Name = "GetTotalExpensesSumPerMonth")]
        public IActionResult GetTotalExpensesSumPerMonth(int month, int year, int userID)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetTotalExpensesSum(month, year, userID));
        }

        [HttpGet(Name = "GetTotalCategories")]
        public IActionResult GetCategories()
        {
            return Ok(m_TotalExpensesPerCategoryService.GetTotalCategories());
        }

        [HttpPost]
        public IActionResult CreateTotalExpensesPerCategories(DateTime fromDate, int userID)
        {            
            List<ExpenseRecord> currentTotalCategoryExpense = m_ExpenseMapperService.GetMapExpenses();
            var totalExpensesToCreate = currentTotalCategoryExpense
                .Where(e => (Convert.ToInt32(e.Linked_Month) == fromDate.Month) && DateTime.Parse(e.Transaction_Date).Year == fromDate.Year).ToList();
            
            var createdTotalExpensesPerCategory = m_TotalExpensesPerCategoryService.CreateTotalExpensesPerCategory(totalExpensesToCreate, fromDate, userID);

            return CreatedAtRoute("GetCategories", new { totalExpensePerCategoryTime = fromDate }, createdTotalExpensesPerCategory);
        }

        [HttpDelete]
        public IActionResult DeleteTotalExpensePerCategory(DateTime timePeriod, string catrgory, int userID)
        {
            var totalExpensesPerCategoryToDelete = m_TotalExpensesPerCategoryService.GetTotalCategories().FirstOrDefault(e => (Convert.ToInt32(e.Month) == timePeriod.Month) 
                                                                                       && e.Year == timePeriod.Year && e.Category == catrgory && e.UserID == userID);
            if(totalExpensesPerCategoryToDelete != null)
            {
                m_TotalExpensesPerCategoryService.DeleteExpensePerCategory(totalExpensesPerCategoryToDelete);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeleteTotalExpensesPerTimePeriod(DateTime fromDate, int userId)
        {
            var totalExpensesPerCategoryToDelete = m_TotalExpensesPerCategoryService.GetTotalCategories();

            if (totalExpensesPerCategoryToDelete != null)
            {
                m_TotalExpensesPerCategoryService.DeleteAllTotalExpensesPerTimePeriod(fromDate,userId);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public IActionResult EditTotalExpensePerCategory(TotalExpensePerCategory expensePerCategory,DateTime timePeriod, string category)
        {
            return Ok(m_TotalExpensesPerCategoryService.EditTotalExpensePerCategory(expensePerCategory, category, timePeriod.Month, timePeriod.Year));
        }        
    }
}
