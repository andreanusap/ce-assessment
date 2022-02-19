using CE.Assessment.BusinessLogic.Entities;

namespace CE.Assessment.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        public OrderService() { }

        public async Task<IEnumerable<OrderDetail>> GetInProgressOrders()
        {
            throw new NotImplementedException();
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

                return orderProducts
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(5);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
