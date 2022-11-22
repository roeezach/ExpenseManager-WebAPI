﻿using Microsoft.AspNetCore.Mvc;
using ExpensesManger.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using ExpensesManager.DB.Models;

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
        public IActionResult GetCategoriesSumPerTimePeriod(int month, int year)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategoriesSum(month, year));
        }

        [HttpGet("{category}/{month:int}/{year:int}",Name = "GetCategoriesSum")]
        public IActionResult GetCategoriesSum(int month, int year, string category)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetCategorySum(month, year, category));
        }

        [HttpGet(Name = "GetTotalExpensesSumPerMonth")]
        public IActionResult GetTotalExpensesSumPerMonth(int month, int year)
        {
            return Ok(m_TotalExpensesPerCategoryService.GetTotalExpensesSum(month, year));
        }

        [HttpGet(Name = "GetTotalCategories")]
        public IActionResult GetCategories()
        {
            return Ok(m_TotalExpensesPerCategoryService.GetTotalCategories());
        }

        [HttpPost]
        public IActionResult CreateTotalExpensesPerCategories(DateTime fromDate)
        {            
            List<ExpenseRecord> currentTotalCategoryExpense = m_ExpenseMapperService.GetMapExpenses();
            var totalExpensesToCreate = currentTotalCategoryExpense
                .Where(e => (Convert.ToInt32(e.Linked_Month) == fromDate.Month) && DateTime.Parse(e.Transaction_Date).Year == fromDate.Year).ToList();
            
            var createdTotalExpensesPerCategory = m_TotalExpensesPerCategoryService.CreateTotalExpensesPerCategory(totalExpensesToCreate, fromDate);

            return CreatedAtRoute("GetCategories", new { totalExpensePerCategoryTime = fromDate }, createdTotalExpensesPerCategory);
        }

        [HttpDelete]
        public IActionResult DeleteTotalExpensePerCategory(DateTime timePeriod, string catrgory)
        {
            var totalExpensesPerCategoryToDelete = m_TotalExpensesPerCategoryService.GetTotalCategories().FirstOrDefault(e => (Convert.ToInt32(e.Month) == timePeriod.Month) 
                                                                                       && e.Year == timePeriod.Year && e.Category == catrgory);
            if(totalExpensesPerCategoryToDelete != null)
            {
                m_TotalExpensesPerCategoryService.DeleteExpensePerCategory(totalExpensesPerCategoryToDelete);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        public IActionResult DeleteTotalExpensesPerTimePeriod(DateTime fromDate)
        {
            var totalExpensesPerCategoryToDelete = m_TotalExpensesPerCategoryService.GetTotalCategories();

            if (totalExpensesPerCategoryToDelete != null)
            {
                m_TotalExpensesPerCategoryService.DeleteAllTotalExpensesPerTimePeriod(fromDate);
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
