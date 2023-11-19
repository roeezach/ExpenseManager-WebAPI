using ExpensesManager.DB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesManager.Automation.Repositories.Mapper;
using ExpensesManager.Automation;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManger.Automation.Utils
{
    public static class ServiceSetup
    {
        public static ServiceProvider SetupServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>();
            serviceCollection.AddScoped<IMapperRepository, MapperRepository>();
            serviceCollection.AddScoped<MapperContext>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
