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
        private Receipt CreateReceipt()
        {
            var receipt = new Receipt()
            {
                Customer = this.Customer,
                CreatedBy = this.CreatedBy,
                CreatedOn = this.CreatedOn,
                TotalRegularPrice = this.TotalRegularPrice,
                TotalSellingPrice = this.TotalSellingPrice,
                Promotions = this.AppliedPromotions.ConvertAll<string>(x => x.ToString())
            };

            receipt.Items = this.Items.ConvertAll<ReceiptItem>(x => new ReceiptItem()
            { 
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                RegularPrice = x.RegularPrice,
                TotalSellingPrice = x.TotalSellingPrice,
                Promotion = x.AppliedPromotion == null ? "" : x.AppliedPromotion.ToString()
            });

            return receipt;
        }
    }
}
