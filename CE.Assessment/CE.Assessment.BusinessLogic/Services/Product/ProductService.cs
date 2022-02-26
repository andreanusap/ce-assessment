using CE.Assessment.BusinessLogic.Entities;
using Newtonsoft.Json;
using CE.Assessment.BusinessLogic.Helpers;
using Microsoft.Extensions.Logging;

namespace CE.Assessment.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IHttpClientHelper httpClientHelper, ILogger<ProductService> logger)
        {
            _httpClientHelper = httpClientHelper;
            _logger = logger;
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
                var productStockRequest = new ProductStockRequest
                {
                    MerchantProductNo = merchantProductNo,
                    Stock = stock
                };

                var requestBody = new List<ProductStockRequest>()
                {
                    productStockRequest
                };

                var serializedDoc = JsonConvert.SerializeObject(requestBody);
                return await _httpClientHelper.HttpPut($"offer/stock", serializedDoc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Stock service failed");
                return false;
            }
        }
    }
}
