/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class ReceiptItem
    {
        public string ProductName { get; set; }             

        public int Quantity { get; set; }

        // Single product regular price,shortcut to ...
        public decimal RegularPrice { get; set; }

        public decimal TotalRegularPrice { get { return this.RegularPrice * this.Quantity; } }

        public decimal TotalSellingPrice { get; set; }

        // Total savings
        public decimal TotalSavings { get { return this.TotalRegularPrice - this.TotalSellingPrice; } }

        // Promotion short description, "" means no promotion applied
        public string Promotion { get; set; }
    }
}
