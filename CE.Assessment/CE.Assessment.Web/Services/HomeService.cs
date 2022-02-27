using AutoMapper;
using CE.Assessment.Shared.Entities;
using CE.Assessment.Web.Models;
using Newtonsoft.Json;
using System.Text;

namespace CE.Assessment.Web.Services
{
    public class HomeService : IHomeService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private ILogger<HomeService> _logger;

        public HomeService(IMapper mapper,
            ILogger<HomeService> logger, 
            HttpClient httpClient)
        {
            _mapper = mapper;
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>Order View Model/returns>
        public async Task<OrderViewModel> GetOrders(int page)
        {
            try
            {
                using var responseMessage = await _httpClient.GetAsync("order");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var orderDetails = await responseMessage.Content.ReadAsAsync<OrderResponse>();

                    if (orderDetails is not null && orderDetails.Count > 0)
                    {
                        double pageCount = orderDetails.TotalCount / orderDetails.Count;
                        var model = _mapper.Map<OrderViewModel>(orderDetails);
                        model.CurrentPage = page;
                        model.PageCount = (int)Math.Ceiling(pageCount);
                        return model;
                    }
                    else
                    {
                        return _mapper.Map<OrderViewModel>(orderDetails);
                    }
                }
                else
                {
                    return new OrderViewModel();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to Get Order");
                throw;
            }
        }

        /// <summary>
        /// Get Top 5 Ordered Products
        /// </summary>
        /// <returns>List of order product view model</returns>
        public async Task<IEnumerable<OrderProductViewModel>> GetTop5Orders()
        {
            try
            {
                var topProducts = new List<OrderProductViewModel>();

                using var responseMessage = await _httpClient.GetAsync("order/top5-ordered");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsAsync<IEnumerable<OrderProductViewModel>>();

                    if (content is not null && content.Any())
                    {
                        topProducts = content.ToList();
                    }
                }

                return topProducts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top 5 products");
                throw;
            }
        }

        /// <summary>
        /// Update Ordered Product
        /// </summary>
        /// <param name="merchantProductNo">Merchant product number</param>
        /// <param name="stock">Stock</param>
        /// <returns>The success of failure of the update ordered product</returns>
        public async Task<bool> UpdateOrderedProduct(string merchantProductNo, int stock)
        {
            try
            {
                var productStockRequest = new ProductStockRequest
                {
                    MerchantProductNo = merchantProductNo,
                    Stock = stock
                };

                var requestJson = JsonConvert.SerializeObject(productStockRequest);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                using var responseMessage = await _httpClient.PutAsync($"product", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to Update");
                throw;
            }
        }
    }
}
