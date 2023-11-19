using BoDi;
using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;
using System;
using ExpenseManger.Automation.Utils;

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

            _objectContainer.RegisterInstanceAs(serviceProvider);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _scenarioScope.Dispose();
        }
    }
}
