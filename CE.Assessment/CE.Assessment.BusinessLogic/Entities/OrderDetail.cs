using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE.Assessment.BusinessLogic.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public int ChannelId { get; set; }
        public string GlobalChannelName { get; set; }
        public int GlobalChannelId { get; set; }
        public string ChannelOrderSupport { get; set; }
        public string ChannelOrderNo { get; set; }
        public string MerchantOrderNo { get; set; }
        public string Status { get; set; }
        public bool IsBusinessOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string MerchantComment { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public double SubTotalInclVat { get; set; }
        public double SubTotalVat { get; set; }
        public double ShippingCostsVat { get; set; }
        public double TotalInclVat { get; set; }
        public double TotalVat { get; set; }
        public double OriginalSubTotalInclVat { get; set; }
        public double OriginalSubTotalVat { get; set; }
        public double OriginalShippingCostsInclVat { get; set; }
        public double OriginalShippingCostsVat { get; set; }
        public double OriginalTotalInclVat { get; set; }
        public double OriginalTotalVat { get; set; }
        public List<Line> Lines { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CompanyRegistrationNo { get; set; }
        public string VatNo { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentReferenceNo { get; set; }
        public double ShippingCostsInclVat { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string ChannelCustomerNo { get; set; }
        public ExtraData ExtraData { get; set; }
    }
}
