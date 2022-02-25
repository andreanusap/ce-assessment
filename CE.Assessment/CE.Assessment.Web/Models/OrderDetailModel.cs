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
        public decimal SubTotalInclVat { get; set; }
        public decimal SubTotalVat { get; set; }
        public decimal ShippingCostsVat { get; set; }
        public decimal TotalInclVat { get; set; }
        public decimal TotalVat { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public decimal ShippingCostsInclVat { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime OrderDate { get; set; }
        public BillingAddressModel BillingAddress { get; set; }
        public ShippingAddressModel ShippingAddress { get; set; }
        public List<LineModel> Lines { get; set; }
    }
}
