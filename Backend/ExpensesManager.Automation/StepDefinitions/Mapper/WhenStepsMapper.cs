using ExpensesManager.DB.Models;
using TechTalk.SpecFlow;
using System;
using System.Collections.Generic;
using ExpensesManager.Automation.Repositories.Mapper;
using ExpensesManager.Automation;
using ExpenseManager.Automation.Utils;

namespace ExpenseManager.Automation.StepDefinitions.Mapper
{

    [Binding]
    public class WhenStepsMapper
    {
        private ScenarioContext _scenarioContext;
        private IMapperRepository _mapperRepository;

        public WhenStepsMapper(ScenarioContext context)
        {
            _scenarioContext = context;
            _mapperRepository = _scenarioContext.Get<IMapperRepository>("Database");
        }

        [When(@"there is data for (.*) and (.*) in the dev database")]
        public void WhenThereIsDataForAndInTheDevDatabase(string month, string year)
        {
            _mapperRepository.SetMapRecordsBasedOnDate(month, year);
            List<ExpenseRecord> MapRecords = _mapperRepository.GetMappedRercords();
            if (MapRecords.Count == 0)
                NUnit.Framework.Assert.Inconclusive($"there are no expenses in this month: {month} and year: {year}");
        }
    }
}