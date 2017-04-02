using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public interface IOrderPromotionContext : IPromotionContext
    {
        bool IsVIP { get; set; }
        bool IsCityEmployee { get; set; }

        decimal TotalSellingPrice { get; set; }      // current order-level total selling price, promotion does calculation against it
    }
}
