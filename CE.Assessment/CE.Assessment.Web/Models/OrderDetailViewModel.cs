namespace CE.Assessment.Web.Models
{
    public class OrderDetailViewModel
    {
        public string ChannelOrderNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDate { get; set; }
        public string SubTotalVat { get; set; }
        public string SubTotalInclVat { get; set; }
        public string ShippingCostsVat { get; set; }
        public string ShippingCostsInclVat { get; set; }
        public string TotalVat { get; set; }
        public string TotalInclVat { get; set; }
    }
}
