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
    /// Order-LEVEL promotion
    /// </summary>
    public abstract class BaseOrderPromotion : BasePromotion, IOrderPromotion
    {
        protected IOrderPromotionContext context;
        protected override IPromotionContext Context
        {
            set { this.context = value as IOrderPromotionContext; }
        }

        #region implement IOrderPromotion

        #endregion
    }
}
