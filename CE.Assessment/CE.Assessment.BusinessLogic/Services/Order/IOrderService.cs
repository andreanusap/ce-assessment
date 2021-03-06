using CE.Assessment.Shared.Entities;

namespace CE.Assessment.BusinessLogic.Services
{
    public interface IOrderService
    {
        public Task<IEnumerable<OrderDetail>> GetInProgressOrders();
        public Task<IEnumerable<OrderProduct>> GetTop5OrderedProducts(IEnumerable<OrderDetail> orderDetails);
        public Task<OrderResponse> GetOrders(string[] statuses = null, int page = 1);
    }
}
