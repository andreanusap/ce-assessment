using CE.Assessment.BusinessLogic.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Options = CE.Assessment.BusinessLogic.Entities.Options;

namespace CE.Assessment.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly Options _options;

        public OrderService(HttpClient httpClient, IOptions<Options> options) 
        {
            _httpClient = httpClient;
            _options = options.Value;
            _httpClient.BaseAddress = new Uri($"{_options.BaseUrl}/orders");
        }

        public class ContentModel
        {
            public List<OrderDetail> Content { get; set; }
        }

        /// <summary>
        /// Get In Progress orders
        /// </summary>
        /// <returns>List of orders</returns>
        public async Task<IEnumerable<OrderDetail>> GetInProgressOrders()
        {
            try
            {
                var request = $"?statuses=IN_PROGRESS&apikey={_options.ApiKey}";
                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, request);
                using var httpResponse = await _httpClient.SendAsync(httpRequest);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ContentModel>(content);
                    return model is not null ? model.Content : Enumerable.Empty<OrderDetail>();
                } else
                {
                    return Enumerable.Empty<OrderDetail>();
                }
            }
            catch (Exception ex)
            { 
                Console.Error.WriteLine(ex.Message);
                throw;
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

                if (orderDetails is null)
                {
                    return orderProducts;
                }

                var dOrderProduct = new Dictionary<string, OrderProduct>();

                foreach(var order in orderDetails)
                {
                    foreach(var line in order.Lines)
                    {
                        if (dOrderProduct.ContainsKey(line.MerchantProductNo))
                        {
                            dOrderProduct[line.MerchantProductNo].TotalQuantity += line.Quantity;
                        } else
                        {
                            dOrderProduct.Add(line.MerchantProductNo,
                                new OrderProduct(line.MerchantProductNo, line.Description, line.Gtin, line.Quantity));
                        }
                    }
                }

                foreach(var orderProduct in dOrderProduct)
                {
                    orderProducts.Add(orderProduct.Value);
                }

                var top5Products = orderProducts
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(5)
                    .ToList();

                if(top5Products.Count() < 5)
                {
                    for(int i = 0;i < 5 - top5Products.Count(); i++)
                    {
                        top5Products.Add(new OrderProduct("", "", "", 0));
                    }
                }

                return top5Products;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
