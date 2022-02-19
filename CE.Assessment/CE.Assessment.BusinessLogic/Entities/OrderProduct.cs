using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CE.Assessment.BusinessLogic.Entities
{
    public class OrderProduct
    {
        public string MerchantProductNo { get; set; }
        public string ProductName { get; set; }
        public string GTIN { get; set; }
        public int TotalQuantity { get; set; }
    }
}
