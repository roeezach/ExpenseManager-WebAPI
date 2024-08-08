using ExpensesManager.Automation.AutomationUtils;
using System;
using System.Text;

namespace ExpensesManager.Automation.Factories
{
    public static class UrlFactory
    {
        public static TimeSpan Timeout => TimeSpan.FromSeconds(30);

        public static string CreateUrl(string domain)
        {
            return new StringBuilder(CreateBaseUri()).Append(domain).ToString();
        }

        public static string CreateBaseUri()
        {
            bool isRunningInContainer = BuisnessLogic.Core.Utils.IsAppInContainer();
            if (isRunningInContainer)
            {
                StringBuilder builder = new StringBuilder();
                return builder.Append(AutomationConstants.BASE_URL_IN_CONTAINER).Append(AutomationConstants.BASE_PORTֹ_IN_CONTAINER).ToString();
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                return builder.Append(AutomationConstants.BASE_URL).Append(AutomationConstants.BASE_PORT).ToString();
            }
        }

        public static string CreateGetUserUrl(string username, string route)
        {
            return CreateUrlWithQuery(route, $"username={Uri.EscapeDataString(username)}");
        }

        // Create a method for constructing URLs with query parameters
        private static string CreateUrlWithQuery(string path, string query)
        {
            return new Uri(new Uri(CreateBaseUri()), $"{path}?{query}").ToString();
        }

    }
}
