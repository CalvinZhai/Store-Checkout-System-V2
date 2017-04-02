using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public interface IProductPromotionContext : IPromotionContext
    {
        string Product { get; set; }
        decimal Price { get; set; }             // regular price
        int Quantity { get; set; }
        decimal TotalPrice { get; }             // total regular price
    }
}
