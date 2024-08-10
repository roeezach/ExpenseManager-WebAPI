using BoDi;
using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;
using System;
using ExpenseManager.Automation.Utils;
// using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using ExpensesManager.DB;
using ExpensesManager.Automation.Repositories.Users;

namespace ExpenseManager.Testing
{
    [Binding]
    public class SpecFlowHooks
    {
        private readonly IObjectContainer _objectContainer;
        private IServiceScope _scenarioScope;

        public SpecFlowHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var serviceProvider = ServiceSetup.SetupServices();
            _scenarioScope = serviceProvider.CreateScope();

            // Register the service provider with the SpecFlow container
            _objectContainer.RegisterInstanceAs(serviceProvider);

            // Register the DbContext with the SpecFlow container
            var dbContext = _scenarioScope.ServiceProvider.GetService<AppDbContext>();
            
            _objectContainer.RegisterInstanceAs(_scenarioScope.ServiceProvider.GetService<IHttpClient>());
            _objectContainer.RegisterInstanceAs(dbContext);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _scenarioScope.Dispose();
        }

        private static IConfiguration LoadConfig()
        {
            return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        }
    }
}