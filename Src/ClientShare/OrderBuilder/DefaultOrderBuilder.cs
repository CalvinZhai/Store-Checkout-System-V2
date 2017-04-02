/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GroceryCo.Checkout.Domain;

namespace GroceryCo.Checkout.Client
{
    public class DefaultOrderBuilder
    {
        // products: "duplicated" products with same .Name will be merged into one OrderItem
        public Order Build(List<Product> basketItems, List<IPromotion> promotions, Customer customer)
        {
            // create order
            var order = new Order() { Customer = customer, Promotions = promotions };

            var items = basketItems.GroupBy(x => x.Name)
                                   .Select(x => new OrderItem() { Product = x.First(), Quantity = x.Count() })
                                   .ToList<OrderItem>();
            order.Items = items;        // resolved, handy for debugging

            return order;
        }
    }
}
