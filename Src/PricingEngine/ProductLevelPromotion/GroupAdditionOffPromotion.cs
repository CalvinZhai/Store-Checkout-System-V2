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
    // e.g. <Promotion Name="GroupPriced" Rule="3-2-40"/> means "Buy 3 Get 2 For 40% Off"
    public class GroupAdditionOffPromotion : GroupPricedPromotion
    {
        public override string Desc { get { return "Products purchased which reach a specified quantity will discount an additional product e.g. 'Buy 1 get 1 for 50% off'"; } }

        // Buy .buy(amount) Get .add(itional amount) For .off(%) Off, e.g. Buy 3 add/get 2 for 40(%) off
        protected new class RuleModel : BaseRuleModel
        {
            public int buy { get; set; }        // e.g. 3
            public int add { get; set; }        // e.g. 2
            public int off { get; set; }        // e.g. 40
        }
        protected new RuleModel RuleObj;

        protected override void ValidateRule(string rule)
        {
            bool good;
            int buyAmount, addAmount, offRatio;

            var factors = rule.Split('-');
            if (factors.Length < 3)
                throw new InvalidPromotionRuleException("GroupAdditionOffPromotion: invaid .Rule string, expect 3 factors in format 'int-int-int'");

            good = int.TryParse(factors[0], out buyAmount);
            if (!good || buyAmount <= 0)
                throw new InvalidPromotionRuleException("GroupAdditionOffPromotion: .buy factor needs > 0");

            good = int.TryParse(factors[1], out addAmount);
            if (!good || addAmount <= 0)
                throw new InvalidPromotionRuleException("GroupAdditionOffPromotion: .add factor needs > 0");

            good = int.TryParse(factors[2], out offRatio);
            if (!good || offRatio <= 0)
                throw new InvalidPromotionRuleException("GroupAdditionOffPromotion: .off factor needs > 0");
        }

        protected override void ParseRule()
        {
            var factors = this.Rule.Split('-');
            var buyAmount = int.Parse(factors[0]);
            var addAmount = int.Parse(factors[1]);
            var offRatio = int.Parse(factors[2]);

            this.RuleObj = new RuleModel() { buy = buyAmount, add = addAmount, off = offRatio };
        }

        public override string ToString()
        {
            return string.Format("Buy {0}, Get {1} For {2}% Off", this.RuleObj.buy, this.RuleObj.add, this.RuleObj.off);
        }

        // Calculate the total price to be paid
        protected override decimal Execute()
        {
            var ret = decimal.MinusOne;

            // Get group # by (this.ruleModel.buy + this.ruleModel.add)
            var quotient = this.context.Quantity / (this.RuleObj.buy + this.RuleObj.add);
            var remainder = this.context.Quantity % (this.RuleObj.buy + this.RuleObj.add);

            if (quotient > 0)       // promotion applicable
            {
                ret = quotient * (
                                    this.RuleObj.buy * this.context.Price
                                    +
                                    this.RuleObj.add * this.context.Price * (100 - this.RuleObj.off) / 100
                                  );

                // handle remainder part
                if (remainder >= this.RuleObj.buy)
                    ret += (
                                this.RuleObj.buy * this.context.Price
                                +
                                (remainder - this.RuleObj.buy) * this.context.Price * (100 - this.RuleObj.off) / 100
                            );

                else
                    ret += remainder * this.context.Price;
            }
            else                    // =0, Quantity < (.buy + .free)
            {
                if (this.context.Quantity > this.RuleObj.buy)
                    ret = (
                              this.RuleObj.buy * this.context.Price
                              +
                              (this.context.Quantity - this.RuleObj.buy) * this.context.Price * (100 - this.RuleObj.off) / 100
                          );
            }

            return ret;
        }
    }
}
