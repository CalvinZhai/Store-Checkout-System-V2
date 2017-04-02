/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    /// <summary>
    /// Store Product
    /// </summary>
    public class Product : BaseDomainObject
    {
        // Product code
        public string Code { get; set; }                // not used so far

        // Product name
        public string Name { get; set; }

        // Current/regular price prior to have any discount/promotion applied
        public decimal Price { get; set; }              // >0 expected

        // Expiry day -- not used by this app but just show common properties a product might have
        public DateTime ExpiryOn { get; set; }                      // not used so far

        public Product() { }

    }
}
