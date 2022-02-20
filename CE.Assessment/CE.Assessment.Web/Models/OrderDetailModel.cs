namespace CE.Assessment.Web.Models
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public string GlobalChannelName { get; set; }
        public string ChannelOrderNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string Status { get; set; }
        public string SubTotalInclVat { get; set; }
        public string SubTotalVat { get; set; }
        public string ShippingCostsVat { get; set; }
        public string TotalInclVat { get; set; }
        public string TotalVat { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingCostsInclVat { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
