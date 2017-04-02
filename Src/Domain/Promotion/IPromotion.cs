using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public interface IPromotion
    {
        string Desc { get; }
        string Rule { get; set; }                   // reference to Data.Basket1.xml for examples

        // Calculate and return the total price to be paid for gving promotionContext
        // NOTE: when returned value < 0, means the promotion not applicable to promotionContext
        decimal Apply(IPromotionContext promotionContext);
    }
}
