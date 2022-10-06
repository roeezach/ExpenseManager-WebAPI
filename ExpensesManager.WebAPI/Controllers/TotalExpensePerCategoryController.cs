using Microsoft.AspNetCore.Mvc;
using ExpensesManger.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using ExpensesManager.DB.Models;

namespace ExpensesManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult GetCategoriesSumPerTimePeriod(int month, int year)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategoriesSum(month, year));
        }

        [HttpGet("{category}/{month:int}/{year:int}",Name = "GetCategoriesSum")]
        public IActionResult GetCategoriesSum(int month, int year, string category)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategorySum(month, year, category));
        }

        [HttpGet(Name = "GetCategories")]
        public IActionResult GetCategories()
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategories());
        }

        [HttpPost]
        public IActionResult CreateTotalExpensesPerCategories(DateTime timePeriod)
        {
            List<ExpenseRecord> currentTotalCategoryExpense = m_ExpenseMapperService.GetMapExpenses();
            var totalExpensesToCreate = currentTotalCategoryExpense
                .Where(e => (Convert.ToInt32(e.Linked_Month) == timePeriod.Month) && DateTime.Parse(e.Transaction_Date).Year == timePeriod.Year).ToList();
            
            var createdTotalExpensesPerCategory = m_TotalExpensesPerCategoryService.CreateTotalExpensesPerCategory(totalExpensesToCreate);

            return CreatedAtRoute("GetCategories", new { totalExpensePerCategoryTime = timePeriod }, createdTotalExpensesPerCategory);
        }

        [HttpDelete]
        public IActionResult DeleteTotalExpensePerCategory(DateTime timePeriod, string catrgory)
        {
            var totalExpensesPerCategoryToDelete = m_TotalExpensesPerCategoryService.GetCategories().FirstOrDefault(e => (Convert.ToInt32(e.Month) == timePeriod.Month) 
                                                                                       && e.Year == timePeriod.Year && e.Category == catrgory);
            if(totalExpensesPerCategoryToDelete != null)
            {
                m_TotalExpensesPerCategoryService.DeleteExpensePerCategory(totalExpensesPerCategoryToDelete);
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
