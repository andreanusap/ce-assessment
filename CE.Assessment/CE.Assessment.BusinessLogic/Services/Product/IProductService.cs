namespace CE.Assessment.BusinessLogic.Services
{
    public interface IProductService
    {
        public Task<bool> UpdateStock(string merchantProductNo, int stock);
    }
}
