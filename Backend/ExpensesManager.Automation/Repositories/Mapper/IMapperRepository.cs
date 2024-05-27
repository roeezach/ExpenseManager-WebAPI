using System;
using System.Collections.Generic;
using ExpensesManager.DB.Models;

namespace ExpensesManager.Automation.Repositories.Mapper
{
    public interface IMapperRepository
    {
        void SetMapRecordsBasedOnDate(string month, string year);
        List<ExpenseRecord> GetMappedRercords();
        int GetChargeDay(int userId);
    }
}

