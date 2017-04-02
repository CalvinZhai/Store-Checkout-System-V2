/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using GroceryCo.Checkout.Framework;
using GroceryCo.Checkout.Domain;

namespace GroceryCo.Checkout.Domain
{
    /// <summary>
    /// Product-LEVEL promotion
    /// Promotion class naming convention: has to end with "Promotion", 
    /// but please remove ending word "Promotion" when configuring Promotion name in product catalog
    /// </summary>
    public abstract class BaseProductPromotion : BasePromotion, IProductPromotion
    {
        protected IProductPromotionContext context;
        protected override IPromotionContext Context
        {
            set { this.context = value as IProductPromotionContext; }
        }

        public BaseProductPromotion()
        {
            this.Products = new List<string>();
        }

        #region implement IProductPromotion

        public List<string> Products { get; set; }

        protected override bool Applicable()
        {
            var productName = this.context.Product;
            if (this.Products.Exists(x=>x.Trim().StartsWith("*-")))         // e.g. "*-apple,orange" means apply to all except ...
            {
                string excludedStr = this.Products.Where(x=>x.Trim().StartsWith("*-")).First();
                excludedStr = excludedStr.Substring(2);
                var exclusives = excludedStr.Split(',');
                if (exclusives.Where(x => x.Trim().ToUpper() == this.context.Product.Trim().ToUpper()).Count() > 0)
                    return false;
                else
                    return true;

            }
            else           // lower priority
                return this.Products.Contains("*") || this.Products.Where(x => x.Trim().ToUpper() == productName.Trim().ToUpper()).Count() > 0; 
        }

        #endregion

    }
}
