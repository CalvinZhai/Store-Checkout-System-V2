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
    // e.g. <Promotion Name="GroupPriced" Rule="3-2.0"/> means "Buy 3 At $2.0"
    public class GroupPricedPromotion : BaseProductPromotion
    {
        public override string Desc { get { return "Group promotional price based on the quantity will have a discounted price, e.g. 'Buy 3 Apple for $2.00'"; } }

        // Buy .buy(amount) at .at($) e.g. Buy 3 at $2.00
        protected new class RuleModel : BaseRuleModel
        {
            public int buy { get; set; }        // 
            public decimal at { get; set; }     // 
        }
        protected new RuleModel RuleObj;

        protected override void ValidateRule(string rule)
        {
            bool good;
            int buyAmount;
            decimal atPrice;

            var factors = rule.Split('-');
            if (factors.Length < 2)
                throw new InvalidPromotionRuleException("GroupPricedPromotion: invalid .Rule string, expect 2 factors in format 'int-decimal'");

            good = int.TryParse(factors[0], out buyAmount);
            if ( !good || buyAmount <= 0 )
                throw new InvalidPromotionRuleException("GroupPricedPromotion: .buy factor needs > 0");

            good = decimal.TryParse(factors[1], out atPrice);
            if (!good || atPrice <= decimal.Zero)
                throw new InvalidPromotionRuleException("GroupPricedPromotion: .at factor needs > 0");
        }

        protected override void ParseRule()
        {
            var factors = this.Rule.Split('-');

            var buyAmount = int.Parse(factors[0]);
            var atPrice = decimal.Parse(factors[1]);

            this.RuleObj = new RuleModel() { buy = buyAmount, at = atPrice };
        }

        public override string ToString()
        {
            return string.Format("Buy {0} For ${1}", this.RuleObj.buy, this.RuleObj.at);
        }

        // Calculate the total price to be paid
        protected override decimal Execute()
        {
            var ret = decimal.MinusOne;

            var quotient = this.context.Quantity / this.RuleObj.buy;
            var remainder = this.context.Quantity % this.RuleObj.buy;

            if (quotient > 0)       // promotion applicable
                ret = quotient * this.RuleObj.at + remainder * this.context.Price;

            return ret;
        }
    }
}
