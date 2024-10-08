﻿using ExpensesManager.Services;
using ExpensesManager.DB;
using System.Data;
using System.Linq;
using ExpensesManager.DB.Models;
using ExpensesManager.BuisnessLogic.Core;
using ExpensesManager.Services.BuisnessLogic.Map;
using ExpensesManager.Services.BuisnessLogic.Map.Common;
using ExpensesManager.Services.Contracts;
using Microsoft.AspNetCore.Http;


namespace ExpensesManager.Services.Services
{
    public class ExpenseMapperService : IExpenseMapperService
    {

        #region Members
        private readonly AppDbContext m_AppDbContext;
        private readonly ICategoryService m_CategoryService;
        private readonly ExpenseMapperFactory m_ExpenseMapperFactory;
        private readonly IServiceProvider m_ServiceProvider;
        private readonly Users m_currUser;


        #endregion
        public ExpenseMapper Mapper { get; private set; }
        public CategoryExpenseMapper CategoryExpense { get; set; }

        #region Ctor

        public ExpenseMapperService(AppDbContext context, ICategoryService categoryService,
        ExpenseMapperFactory expenseMapperFactory, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            m_AppDbContext = context;
            m_ExpenseMapperFactory = expenseMapperFactory;
            m_CategoryService = categoryService;
            m_ServiceProvider = serviceProvider;
            Mapper = new ExpenseMapper(serviceProvider);
            CategoryExpense = expenseMapperFactory.GetMapper<CategoryExpenseMapper>(m_ServiceProvider);
            m_currUser = m_AppDbContext.Users.FirstOrDefault(u => u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        #endregion

        #region Public Methods

        public List<ExpenseRecord> GetMapExpenses()
        {
            return m_AppDbContext.Expenses.ToList();
        }

        public List<ExpenseRecord> GetMapExpensesPerMonth(int currentExpenseMonth, int currentExpenseYear, int userID)
        {
            return GetExpensesePerMonthAndYear(currentExpenseMonth, currentExpenseYear, userID);
        }

        public void DeleteExpenses(int currentExpenseMonth, int currentExpenseYear, int userID)
        {
            var expensesPerMonth = GetExpensesePerMonthAndYear(currentExpenseMonth, currentExpenseYear, userID);

            foreach (var expense in expensesPerMonth)
            {
                m_AppDbContext.Remove(expense);
                m_AppDbContext.SaveChanges();
            }
        }

        public List<ExpenseRecord> CreateExpenses(DataTable dataTable, BankTypes.FileTypes fileType, int userID, DateTime ChargeDate)
        {
            List<ExpenseMapper>? mappedExpenses = Mapper.MapFile(dataTable, fileType, userID, m_ExpenseMapperFactory);
            List<ExpenseRecord> expensesRecords = new();
            foreach (var expense in mappedExpenses)
            {
                expensesRecords.Add(SetExpenseMapperToExpenseRecord(expense, userID, ChargeDate));
            }
            m_AppDbContext.AddRange(expensesRecords);
            m_AppDbContext.SaveChanges();

            return expensesRecords;
        }

        public ExpenseRecord EditExpense(ExpenseRecord editedExpense, int expenseID)
        {
            var expense = m_AppDbContext.Expenses.FirstOrDefault(e => e.TransactionID == expenseID);

            expense = editedExpense;
            m_AppDbContext.SaveChanges();

            return expense;
        }

        #endregion

        #region Internal Methods

        internal ExpenseRecord SetExpenseMapperToExpenseRecord(ExpenseMapper expenseMapper, int userID, DateTime chargedDate)
        {
            string month = expenseMapper.TransactionDate.HasValue ? Utils.GetExpenseLinkedMonth(expenseMapper.TransactionDate.Value, chargedDate).ToString() : "-1";
            string? exchangeDescription = Utils.ReformatHebrewString(expenseMapper.ExchangeDescription);

            ExpenseRecord expenseRecord = new()
            {
                TransactionID = Utils.GenerateRandomID(),
                Transaction_Date = expenseMapper.TransactionDate.ToString(),
                Card_Details = expenseMapper.CardDetails,
                Record_Create_Date = DateTime.Now.ToString(),
                Expense_Description = expenseMapper.ExpenseDescription,
                Price_Amount = expenseMapper.PriceAmount,
                Debit_Amount = expenseMapper.DebitAmount,
                DebitCurrency = expenseMapper.DebitCurrency,
                Exchange_Rate = expenseMapper.ExchangeRate,
                Exchange_Description = exchangeDescription,
                Category = expenseMapper.CategoryData.CategoryKey,
                Linked_Month = month,
                User_ID = userID,
                Linked_Year = Utils.GetExpenseLinkedYearAsDateTime(expenseMapper.TransactionDate.Value, Utils.GetUserChargeDay(userID, m_AppDbContext), Convert.ToInt32(month)).Year.ToString()
            };

            if (!string.IsNullOrEmpty(expenseMapper.ExchangeDescription))
            {
                expenseRecord.Debit_Amount = double.Parse(expenseMapper.ForegnierDebitAmount);
                expenseRecord.Exchange_Rate = Utils.GetExchangeRate(exchangeDescription);
            }

            return expenseRecord;
        }

        internal List<ExpenseRecord> GetExpensesePerMonthAndYear(int currentExpenseMonth, int currentExpenseYear, int userID)
        {
            var expensesPerMonth = Mapper.GetExpensesePerMonthAndYear(m_AppDbContext.Expenses.ToList(), currentExpenseMonth, currentExpenseYear, userID);
            return expensesPerMonth.Where(e => e.Linked_Year == currentExpenseYear.ToString()).ToList();
        }

        #endregion
    }
}