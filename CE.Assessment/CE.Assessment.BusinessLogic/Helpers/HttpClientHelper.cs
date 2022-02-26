using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Options = CE.Assessment.Shared.Entities.Options;

namespace CE.Assessment.BusinessLogic.Helpers
{
    public class HttpClientHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly Options _options;
        private readonly ILogger<HttpClientHelper> _logger;

        public HttpClientHelper(HttpClient httpClient, IOptions<Options> options, ILogger<HttpClientHelper> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}");
            _logger = logger;
        }

        public async Task<T> HttpGet<T>(string uri, string requestParam = null)
        {
            try
            {
                var request = $"{uri}?apikey={_options.ApiKey}{requestParam}";

                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
                using var httpResponse = await _httpClient.SendAsync(httpRequest);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Http Get failed");
                throw;
            }
        }

        public async Task<bool> HttpPut(string uri, string serializedDoc = null)
        {
            try
            {
                var request = $"{uri}?apikey={_options.ApiKey}";
                using var httpRequest = new HttpRequestMessage(HttpMethod.Put, request);
                httpRequest.Content = new StringContent(serializedDoc, Encoding.UTF8, "application/json");
                using var httpResponse =  await _httpClient.SendAsync(httpRequest);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Http Put failed");
                throw;
            }
        }
    }
}
