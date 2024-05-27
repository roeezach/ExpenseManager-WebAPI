namespace ExpensesManager.Services.Services
{
    public static class ServiceFactory
    {
        public static T GetService<T>(IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
