/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class Receipt : BaseDomainObject
    {
        public Customer Customer { get; set; }

        public Cashier CreatedBy { get; set; }              // Store staff who created the order
        public DateTime CreatedOn { get; set; }

        public List<ReceiptItem> Items { get; set; }

        public decimal TotalRegularPrice { get; set; }
        public decimal TotalSellingPrice { get; set; }      // 
        public decimal TotalSavings
        {
            get { return this.TotalRegularPrice - this.TotalSellingPrice; }
        }

        // order level promotions applied
        public List<string> Promotions { get; set; }

        public Receipt()
        {
            this.Items = new List<ReceiptItem>();
            this.Promotions = new List<string>();
        }
    }
}
