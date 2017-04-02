/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    // Same Product (name) would be merged into a single OrderItem
    public partial class OrderItem : BaseDomainObject
    {
        // Promotion actually applied to OrderItem -- maxmum ONLY one
        private IProductPromotion appliedPromotion;
        public IProductPromotion AppliedPromotion { get { return this.appliedPromotion; } }     

        public Product Product { get; set; }

        public int Quantity { get; set; }           // >0  expected

        // Single product regular price,shortcut to ...
        public decimal RegularPrice { get {  return this.Product.Price; } }

        public decimal TotalRegularPrice { get { return this.RegularPrice * this.Quantity; } }

        // After promotions applied (if having)
        //private decimal totalSellingPrice = Decimal.Zero;
        //public decimal TotalSellingPrice { get { return this.totalSellingPrice; } }
        public decimal TotalSellingPrice { get; set; }

        // Total savings
        public decimal TotalSavings { get { return this.TotalRegularPrice - this.TotalSellingPrice; } }
    }
}
