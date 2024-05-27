using BoDi;
using TechTalk.SpecFlow;
using Microsoft.Extensions.DependencyInjection;
using System;
using ExpenseManager.Automation.Utils;

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
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                Console.WriteLine($"Resolving: {e.Name}");
                return null;
            };
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _scenarioScope.Dispose();
        }
    }
}
