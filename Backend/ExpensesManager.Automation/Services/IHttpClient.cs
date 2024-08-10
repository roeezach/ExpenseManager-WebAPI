public interface IHttpClient
{
    Task<HttpResponseMessage> GetAsync(string endpoint);
    Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content);
    Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content);
    Task<HttpResponseMessage> DeleteAsync(string endpoint);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}
