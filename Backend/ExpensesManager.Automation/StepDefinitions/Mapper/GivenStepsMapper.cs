using BoDi;
using ExpensesManager.DB;
using ExpensesManager.DB.Models;
using ExpensesManager.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using ExpensesManager.Automation.Repositories.Mapper;
using ExpensesManager.Automation;
using ExpenseManager.Automation.Utils;

namespace ExpenseManager.Automation.StepDefinitions.Mapper
{
    [Binding]
    public class GivenStepsMapper
    {
        private ScenarioContext _scenarioContext;
        private AppDbContext _appDbContextAutomation;
        private MapperContext _mapperContext;
        private IMapperRepository _mapperRepository;

        public GivenStepsMapper(ScenarioContext scenrioContex, AppDbContext appDbContext)
        {
            // var serviceProvider = ServiceSetup.SetupServices();
            _scenarioContext = scenrioContex;
            _appDbContextAutomation = appDbContext;
            _mapperRepository = new MapperRepository(appDbContext);
            _scenarioContext.Add("Database", _mapperRepository);
        }

        [Given(@"I have a SQLite database")]
        public void GivenIHaveASQLiteDatabase()
        {
            _appDbContextAutomation.Database.EnsureCreated();
        }
    }
}