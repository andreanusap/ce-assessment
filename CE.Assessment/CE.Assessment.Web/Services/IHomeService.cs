using CE.Assessment.Web.Models;

namespace CE.Assessment.Web.Services
{
    public interface IHomeService
    {
        Task<OrderViewModel> GetOrders(int page);
        Task<IEnumerable<OrderProductViewModel>> GetTop5Orders();
        Task<bool> UpdateOrderedProduct(string merchantProductNo, int stock);
    }
}
