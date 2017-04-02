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
    // e.g. <Promotion Name="OnSalePriced" Rule="1.23"/> means applied product is on sale at $1.23
    public class OnSalePricedPromotion : BaseProductPromotion
    {
        public override string Desc { get { return "Selected products are purchased at a discounted price"; } }

        // slightly different naming convention
        protected new class RuleModel : BaseRuleModel
        {
            public decimal onSellAt { get; set; }       // on sell at this price, needs > 0
        }
        protected new RuleModel RuleObj;

        protected override void ValidateRule(string rule) 
        { 
            decimal price;

            var good = decimal.TryParse(rule, out price);
            if ( !good || price <= decimal.Zero )
                throw new InvalidPromotionRuleException("OnSalePriced Promotion: decimal(>0) expected");
        }

        protected override void ParseRule() 
        {
            this.RuleObj = new RuleModel() { onSellAt = decimal.Parse(this.Rule) };
        }

        public override string ToString()
        {
            return string.Format("On Sale At ${0}", this.RuleObj.onSellAt);
        }

        // Calculate the total price to be paid
        protected override decimal Execute()
        {
            return this.RuleObj.onSellAt * this.context.Quantity;
        }
    }
}
