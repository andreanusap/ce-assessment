namespace CE.Assessment.Web.Models
{
    public class OrderResponseModel
    {
        public List<OrderDetailModel> Content { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
    }
}
