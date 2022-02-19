using CE.Assessment.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE.Assessment.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        public OrderService() { }

        public async Task<IEnumerable<OrderDetail>> GetInProgressOrders()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderProduct>> GetTop5OrderedProducts(IEnumerable<OrderDetail> orderDetails)
        {
            throw new NotImplementedException();
        }
    }
}
