namespace CE.Assessment.Shared.Entities
{
    public class OrderProduct
    {
        public string MerchantProductNo { get; set; }
        public string ProductName { get; set; }
        public string Gtin { get; set; }
        public int TotalQuantity { get; set; }

        public OrderProduct(string merchantProductNo, string productName, string gtin, int totalQuantity)
        {
            MerchantProductNo = merchantProductNo;
            ProductName = productName;
            Gtin = gtin;
            TotalQuantity = totalQuantity;
        }
     }
}
