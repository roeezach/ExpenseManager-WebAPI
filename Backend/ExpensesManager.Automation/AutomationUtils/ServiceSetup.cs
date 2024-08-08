using ExpensesManager.DB;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using ExpensesManager.Automation.Repositories.Mapper;
using ExpensesManager.Automation;
using Microsoft.Extensions.Configuration;
using ExpensesManager.Automation.Services;
using ExpensesManager.Automation.Repositories.Users;
using ExpensesManager.Automation.Context;
using ExpensesManager.Automation.AutomationUtils;
using ExpensesManager.Automation.Utilities;

namespace ExpenseManager.Automation.Utils
{
    public static class ServiceSetup
    {
        public static ServiceProvider SetupServices()
        {
            var serviceCollection = new ServiceCollection();

            var basePath = BuildBasePath();
            var builder = new ConfigurationBuilder().SetBasePath(basePath);

            bool isRunningInContainer = ExpensesManager.BuisnessLogic.Core.Utils.IsAppInContainer();

            if (!isRunningInContainer)
            {
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();

            IConfigurationRoot config = builder.Build();
            
            RegisterServices(serviceCollection, config);

            return serviceCollection.BuildServiceProvider();
        }

        private static void RegisterServices(ServiceCollection serviceCollection, IConfigurationRoot config)
        {
            serviceCollection.AddSingleton<IConfiguration>(config);

            serviceCollection.AddDbContext<AppDbContext>();

            // Register other services
            serviceCollection.AddScoped<IMapperRepository, MapperRepository>();
            serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
            serviceCollection.AddScoped<MapperContext>();
            serviceCollection.AddScoped<UsersContext>();
            serviceCollection.AddScoped<HttpResponseLogger>();

            //serviceCollection.AddScoped<IHttpClient, HttpClientWrapper>();

            serviceCollection.AddHttpClient<IHttpClient, HttpClientWrapper>
                (client =>
            {
                StringBuilder baseUri = new StringBuilder();
                baseUri.Append(AutomationConstants.BASE_URL).Append(AutomationConstants.BASE_PORT);
                client.BaseAddress = new Uri(baseUri.ToString());
            });

            // Register ApiClient with Dependency Injection
            //serviceCollection.AddTransient<ApiClient>();
        }

        private static string BuildBasePath()
        {
            bool isRunningInContainer = ExpensesManager.BuisnessLogic.Core.Utils.IsAppInContainer();

            // Use the appropriate base path
            var basePath = isRunningInContainer
                ? Path.Combine(Directory.GetCurrentDirectory(), "Configurations") // Path inside the container
                : @"C:\Users\roeez\OneDrive - cs.colman.ac.il\מסמכים\Development\repos\ExpenseManagerSqlite\ExpenseManager-WebAPI\Backend\ExpensesManager.WebAPI\Configurations"; // Full path on local machine

            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException($"The specified directory does not exist: {basePath}");
            }

            return basePath;
        }        
    }
}
