using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Options = CE.Assessment.BusinessLogic.Entities.Options;

namespace CE.Assessment.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly Options _options;

        public ProductService(HttpClient httpClient, IOptions<Options> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}/offer/");
        }

        /// <summary>
        /// Update product's stock
        /// </summary>
        /// <param name="merchantProductNo">Merchant product number</param>
        /// <param name="stock">Stock</param>
        /// <returns>Boolean indicating successful or failed update</returns>
        public async Task<bool> UpdateStock(string merchantProductNo, int stock)
        {
            try
            {
                var requestUri = $"stock?apiKey={_options.ApiKey}";
                var requestBody = new List<object>()
                {   
                    new
                    {
                        MerchantProductNo = merchantProductNo,
                        Stock = stock
                    }
                };

                var serializedDoc = JsonConvert.SerializeObject(requestBody);
                using var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
                request.Content = new StringContent(serializedDoc, Encoding.UTF8, "application/json");
                using var response = await _httpClient.SendAsync(request);


                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
