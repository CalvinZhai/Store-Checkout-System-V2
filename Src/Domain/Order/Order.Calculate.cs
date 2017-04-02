/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    // Let's put big/complicate method into seperate file to easier maintenance and merge work
    public partial class Order
    {
        // Return Order's final total price to be paied by client
        public decimal Calculate()
        {
            this.ValidatePreCalculate();

            // first: apply product-level promotions to order items
            foreach (var item in this.Items)
            {
                var productPromotions = this.Promotions.Where(x => typeof(IProductPromotion).IsAssignableFrom(x.GetType())).Cast<IProductPromotion>().ToList();
                this.TotalSellingPrice += item.Calculate(productPromotions);
            }

            // then: apply order-level promotions to order
            var orderPromotions = this.Promotions.Where(x => typeof(IOrderPromotion).IsAssignableFrom(x.GetType())).Cast<IOrderPromotion>().ToList();
            foreach (var promotion in orderPromotions)
            {
                var tsp = promotion.Apply(new BaseOrderPromotionContext()
                {
                    IsVIP = typeof(IVIP).IsAssignableFrom(this.Customer.GetType()),
                    IsCityEmployee = typeof(ICityEmployee).IsAssignableFrom(this.Customer.GetType()),
                    TotalSellingPrice = this.TotalSellingPrice
                });

                if (tsp > decimal.MinusOne)              // promotion actually applicable
                {
                    this.AppliedPromotions.Add(promotion);
                    this.TotalSellingPrice = tsp;
                }

            }

            // Generate Receipt
            this.Receipt = this.CreateReceipt();

            return this.TotalSellingPrice;
        }

        private void ValidatePreCalculate()
        {
            foreach (var item in this.Items)
            {
                if (item.Product.Name == "")
                    throw new ProductNameEmptyException("Order.ValidatePreCalculate");

                if (item.RegularPrice <= decimal.Zero)
                    throw new ProductPriceLessThanZeroException(string.Format("Order.ValidatePreCalculate:item {0} regular price expects > 0", item.Product.Name));

                if (item.Quantity <=0)
                    throw new ProductQuantityLessThanZeroException(string.Format("Order.ValidatePreCalculate:item {0} quantity expects > 0", item.Product.Name));
            }
        }
    }
}
