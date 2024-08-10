using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExpensesManager.Automation.Services
{
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            try
            {
                return await _httpClient.GetAsync(endpoint);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error fetching data from {endpoint}: {ex.Message}", ex);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            try
            {
                return await _httpClient.PostAsync(endpoint, content);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error posting data to {endpoint}: {ex.Message}", ex);
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content)
        {
            try
            {
                return await _httpClient.PutAsync(endpoint, content);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error updating data on {endpoint}: {ex.Message}", ex);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            try
            {
                return await _httpClient.DeleteAsync(endpoint);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error deleting data from {endpoint}: {ex.Message}", ex);
            }
        }

        // Implement the SendAsync method
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                return await _httpClient.SendAsync(request);
            }
            catch (HttpRequestException ex)
            {
                // Log or handle exception
                throw new ApplicationException($"Error sending request to {request.RequestUri}: {ex.Message}", ex);
            }
        }   
    }
}
