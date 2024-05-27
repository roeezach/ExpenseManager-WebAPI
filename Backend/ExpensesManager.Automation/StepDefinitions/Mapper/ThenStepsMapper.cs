using ExpensesManager.Automation;
using ExpensesManager.Automation.Repositories.Mapper;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManager.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace ExpenseManager.Automation.StepDefinitions
{
    [Binding]
    public class ThenStepsMapper
    {
        private readonly ScenarioContext _scenrioContex;
        private readonly IMapperRepository _mapperRepository;
        public ThenStepsMapper(ScenarioContext mapperContext )
        {
            _scenrioContex = mapperContext;
            _mapperRepository = _scenrioContex.Get<IMapperRepository>("Database");
        }

        [Then(@"the linked months and linked year should be consistent with the transaction date")]
        public void ThenTheLinkedMonthsAndLinkedYearShouldBeConsistentWithTheTransactionDate()
        {

            var mappedRecords = _mapperRepository.GetMappedRercords();
            var whatINeed = mappedRecords.Where(rec => Int64.Parse(rec.Linked_Month) == 2 && DateUtils.GetDate(rec.Transaction_Date).Month == 1);

            foreach (var mapedRecord in mappedRecords)
            {
                var userChargeDay = _mapperRepository.GetChargeDay(mapedRecord.User_ID);
                if (userChargeDay == 10 && DateUtils.GetDate(mapedRecord.Transaction_Date).Day < 10
                    && DateUtils.GetDate(mapedRecord.Transaction_Date).Month == 1)
                {
                    NUnit.Framework.Assert.AreEqual(Int64.Parse(mapedRecord.Linked_Month), 12, $"transactiod id: {mapedRecord.TransactionID}");
                    NUnit.Framework.Assert.AreEqual(Int64.Parse(mapedRecord.Linked_Year), DateUtils.GetDate(mapedRecord.Transaction_Date).Year - 1, $"transactiod id: {mapedRecord.TransactionID}");
                }

                else if (userChargeDay == 10 && DateUtils.GetDate(mapedRecord.Transaction_Date).Day < 10)
                {
                    NUnit.Framework.Assert.AreEqual(Int64.Parse(mapedRecord.Linked_Month), DateUtils.GetDate(mapedRecord.Transaction_Date).Month - 1, $"transactiod id: {mapedRecord.TransactionID}");
                    NUnit.Framework.Assert.AreEqual(Int64.Parse(mapedRecord.Linked_Year), DateUtils.GetDate(mapedRecord.Transaction_Date).Year, $"transactiod id: {mapedRecord.TransactionID}");
                }
                else if(mapedRecord.Price_Amount > mapedRecord.Debit_Amount)
                {
                    Console.WriteLine($"Payment deal in {mapedRecord.TransactionID} with description: {mapedRecord.Expense_Description}", $"transactiod id: {mapedRecord.TransactionID}");
                }
                else
                {
                    NUnit.Framework.Assert.AreEqual(Int64.Parse(mapedRecord.Linked_Month), DateUtils.GetDate(mapedRecord.Transaction_Date).Month, $"transactiod id: {mapedRecord.TransactionID}");
                    NUnit.Framework.Assert.AreEqual(Int64.Parse(mapedRecord.Linked_Year), DateUtils.GetDate(mapedRecord.Transaction_Date).Year, $"transactiod id: {mapedRecord.TransactionID}");
                }
            }

        }

    }
}