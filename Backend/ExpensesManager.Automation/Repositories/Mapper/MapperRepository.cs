using System;
using System.Collections.Generic;
using ExpensesManager.DB.Models;
using Microsoft.EntityFrameworkCore;
using ExpensesManager.DB;

namespace ExpensesManager.Automation.Repositories.Mapper
{

    public class MapperRepository : IMapperRepository
    {
        private readonly AppDbContext m_AppDbContext;

        public List<ExpenseRecord> MappedRecords { get; set; }

        public MapperRepository(AppDbContext dbContext)
        {
            m_AppDbContext = dbContext;
        }

        public void SetMapRecordsBasedOnDate(string month, string year)
        {
            MappedRecords = m_AppDbContext.Expenses.Where(record => record.Linked_Month == month && record.Linked_Year == year)
                                                    .ToList();
        }

        public List<ExpenseRecord> GetMappedRercords()
        {
            return MappedRecords;
        }

        public int GetChargeDay(int userId)
        {
            return m_AppDbContext.Users.FirstOrDefault(usr => usr.UserID == userId).CreditCardChargeDay;
        }

    }
}
