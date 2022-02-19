using CE.Assessment.BusinessLogic.Entities;

namespace CE.Assessment.BusinessLogic.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDetail>> GetInProgressOrders();
        public Task<IEnumerable<OrderProduct>> GetTop5OrderedProducts(IEnumerable<OrderDetail> orderDetails);
    }
}
