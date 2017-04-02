using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class BaseProductPromotionContext : BaseDomainObject, IProductPromotionContext
    {
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice
        {
            get { return this.Price * this.Quantity; }
        }
    }
}
