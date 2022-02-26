using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Helpers;
using Microsoft.Extensions.Logging;

namespace CE.Assessment.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IHttpClientHelper httpClientHelper, ILogger<OrderService> logger) 
        {
            _httpClientHelper = httpClientHelper;
            _logger = logger;
        }

        /// <summary>
        /// Get In Progress orders
        /// </summary>
        /// <returns>List of orders</returns>
        public async Task<IEnumerable<OrderDetail>> GetInProgressOrders()
        {
            try
            {
                var model = await _httpClientHelper.HttpGet<OrderResponse>("orders", "&statuses=IN_PROGRESS");
                return model.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get In Progress service failed");
                return null;
            }
        }

        /// <summary>
        /// Get top 5 products from orders
        /// </summary>
        /// <param name="orderDetails">List of order</param>
        /// <returns>List of order products</returns>
        public async Task<IEnumerable<OrderProduct>> GetTop5OrderedProducts(IEnumerable<OrderDetail> orderDetails)
        {
            try
            {
                var orderProducts = new List<OrderProduct>();

                if (orderDetails is null || !orderDetails.Any())
                {
                    return orderProducts;
                }

                var lines = orderDetails.SelectMany(o => o.Lines).ToList();
                return lines.GroupBy(l => l.MerchantProductNo)
                    .Select(r => new OrderProduct(r.First().MerchantProductNo, 
                        r.First().Description,
                        r.First().Gtin, 
                        r.Sum(c => c.Quantity)))
                    .OrderByDescending(p => p.TotalQuantity)
                    .Take(5)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Top 5 Ordered Products service failed");
                return null;
            }
        }

        /// <summary>
        /// Get orders by status and page
        /// </summary>
        /// <param name="statuses">List of status</param>
        /// <param name="page">Page</param>
        /// <returns>Order response</returns>
        public async Task<OrderResponse> GetOrders(string[] statuses = null, int page = 1)
        {
            try
            {
                var requestParam = BuildRequestParameter(statuses: statuses, page: page);
                return await _httpClientHelper.HttpGet<OrderResponse>("orders", requestParam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get Orders service failed");
                return null;
            }
        }

        /// <summary>
        /// Build request parameters
        /// </summary>
        /// <param name="statuses">Array of status</param>
        /// <param name="page">Page number</param>
        /// <returns>String of request parameter</returns>
        private string BuildRequestParameter(string[] statuses = null, int page = 1)
        {
            var requestParam = string.Empty;

            if(statuses is not null && statuses.Length > 0)
            {
                for(int i = 0; i < statuses.Length; i++)
                {
                    requestParam += $"&statuses={statuses[i]}";
                }
            }

            if(page > 1)
            {
                requestParam += $"&page={page}";
            }

            return requestParam;
        }
    }
}
