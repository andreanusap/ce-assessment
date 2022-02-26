namespace CE.Assessment.Shared.Entities
{
    public class OrderResponse
    {
        public List<OrderDetail> Content { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
    }
}
