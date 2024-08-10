using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ExpensesManager.Automation.Utilities
{
    public class RequestBuilder<T>
    {
        private readonly HttpRequestMessage _request;

        public RequestBuilder(HttpMethod method, string url)
        {
            _request = new HttpRequestMessage(method, url);
        }

        public RequestBuilder<T> AddJsonContent(T content)
        {
            var jsonData = JsonConvert.SerializeObject(content);
            _request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return this;
        }

        public RequestBuilder<T> AddHeader(string name, string value)
        {
            _request.Headers.Add(name, value);
            return this;
        }

        public HttpRequestMessage Build()
        {
            return _request;
        }
    }
}
