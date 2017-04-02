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
    // e.g. <Promotion Name="GroupPriced" Rule="3-2-40"/> means "Buy 3 Get 2 For Free"
    public class GroupAdditionFreePromotion : GroupAdditionOffPromotion
    {
        public override string Desc { get { return "Products purchased which reach a specified quantity will discount an additional product at free, e.g. 'Buy 3 get 1 free'"; } }

        public override string Rule
        {
            get { return base.Rule; }
            set
            {
                value = value + "-100";     // make to be like '3-2-100'meaning buy 3 get 2 for 100% off (free)
                base.Rule = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Buy {0}, Get {1} For Free", this.RuleObj.buy, this.RuleObj.add);
        }
    }
}
