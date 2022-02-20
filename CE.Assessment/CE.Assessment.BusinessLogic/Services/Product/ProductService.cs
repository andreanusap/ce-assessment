using CE.Assessment.BusinessLogic.Entities;
using Microsoft.AspNetCore.JsonPatch;
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
            _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}/products");
        }

        /// <summary>
        /// Update product's stock
        /// </summary>
        /// <param name="merchantProductNo">Merchant product number</param>
        /// <param name="patchDoc">JSON patch</param>
        /// <returns></returns>
        public async Task<PatchResponse> UpdateProduct(string merchantProductNo, JsonPatchDocument patchDoc)
        {
            try
            {
                var requestUri = $"/{merchantProductNo}?apiKey={_options.ApiKey}";

                var serializedDoc = JsonConvert.SerializeObject(patchDoc);
                using var request = new HttpRequestMessage(HttpMethod.Patch, requestUri);
                request.Content = new StringContent(serializedDoc, Encoding.UTF8);
                using var httpResponse = await _httpClient.SendAsync(request);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<PatchResponse>(content);
                }
                else
                {
                    return new PatchResponse();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
