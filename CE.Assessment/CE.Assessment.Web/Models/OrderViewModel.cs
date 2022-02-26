namespace CE.Assessment.Web.Models
{
    public class OrderViewModel
    {
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
