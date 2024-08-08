using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExpensesManager.Automation.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Generic method to send HTTP requests
        public async Task<T> SendRequestAsync<T>(HttpMethod method, string endpoint, object content = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (content != null)
            {
                var jsonContent = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseData);
        }

        public Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return _httpClient.GetAsync(endpoint);
        }

        public Task<HttpResponseMessage> PostAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return _httpClient.PostAsync(endpoint, content);
        }

        public Task<HttpResponseMessage> PutAsync(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return _httpClient.PutAsync(endpoint, content);
        }

        public Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return _httpClient.DeleteAsync(endpoint);
        }
    }
}
