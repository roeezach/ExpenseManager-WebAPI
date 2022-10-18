
namespace ExpensesManager.Integrations.GetExpensesAPI
{
    internal static class GetExpensesResponseProcesser
    {
        internal static async Task<string> GetTaskAsyncResponse()
        {
            HttpClient currentClient = GetExpensesRequestBuilder.apiClient;
            using HttpResponseMessage response = await currentClient.GetAsync(currentClient.BaseAddress.AbsoluteUri.ToString());
            if (response.IsSuccessStatusCode)
            {
                string expensesData = await response.Content.ReadAsStringAsync();
                return expensesData;
            }
            else
                throw new  HttpRequestException(response.ReasonPhrase);
        }              
    }
}