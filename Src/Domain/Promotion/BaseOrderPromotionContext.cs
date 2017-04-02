using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class BaseOrderPromotionContext : BaseDomainObject, IOrderPromotionContext
    {
        public bool IsVIP { get; set; }
        public bool IsCityEmployee { get; set; }
        public decimal TotalSellingPrice { get; set; } 
    }
}
