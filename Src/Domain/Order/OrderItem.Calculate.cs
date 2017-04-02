/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    // private
    class OrderItemPromotionContext : BaseProductPromotionContext { }

    // Let's put big/complicate method into seperate file to easier maintenance and merge work
    // Will actually apply the cheapest one when having multiple promotions applicable
    public partial class OrderItem 
    {
        // Calculate OrderItem's total SellingPrice
        // One OrderItem may have multiple IProductPromotion, but only the one who gives the cheapest will actually apply
        public decimal Calculate(IList<IProductPromotion> promotions)
        {
            var totalSelling = this.TotalRegularPrice;

            var cheapestTotal = Decimal.MinusOne;
            IProductPromotion cheapestPromotion = null;
            foreach(var promotion in promotions)
            {
                IProductPromotionContext promotionContext = new OrderItemPromotionContext() 
                { 
                    Product = this.Product.Name, Price = this.RegularPrice, Quantity = this.Quantity 
                };

                var promotionPrice = promotion.Apply(promotionContext);
                if (promotionPrice > 0)                         // promotion actually applicable 
                    if (cheapestTotal == Decimal.MinusOne)      // first applicable promotion
                    {
                        cheapestTotal = promotionPrice;
                        cheapestPromotion = promotion;
                    }
                    else if (promotionPrice < cheapestTotal)    // non-first applicable promotion
                    {
                        cheapestTotal = promotionPrice;
                        cheapestPromotion = promotion;
                    }
            }

            if (cheapestTotal > Decimal.MinusOne)               // has at least one promotion actually applied
            {
                totalSelling = cheapestTotal;
                this.appliedPromotion = cheapestPromotion;
            }

            this.TotalSellingPrice = totalSelling;

            return totalSelling;
        }
    }
}
