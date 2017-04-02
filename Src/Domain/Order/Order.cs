/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public partial class Order : BaseDomainObject
    {
        public Customer Customer { get; set; }

        public Cashier CreatedBy { get; set; }             // Store staff who created the order
        public DateTime CreatedOn { get; set; }

        public List<OrderItem> Items { get; set; }         // order items

        public Receipt Receipt { get; set; }               // Associated receipt

        // if having multi applied promotions, they just stack...
        public List<IOrderPromotion> AppliedPromotions { get; set; }    // actually applied promotions 
        public List<IPromotion> Promotions { get; set; }                // Potiential promotions, some may not applicable

        public decimal TotalRegularPrice { get { return this.Items.Sum(x => x.RegularPrice * x.Quantity); } }

        public decimal TotalSellingPrice { get; set; }

        public decimal TotalSavings
        { get { return this.TotalRegularPrice - this.TotalSellingPrice; } }

        public Order()
        {
            this.Customer = new Customer();
            this.CreatedOn = DateTime.Now;

            this.Items = new List<OrderItem>();
            this.Promotions = new List<IPromotion>();
            this.AppliedPromotions = new List<IOrderPromotion>();
        }
    }
}
