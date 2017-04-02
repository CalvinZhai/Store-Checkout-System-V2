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
    // e.g. <Promotion Name="OnSaleOff" Rule="40"/> means applied product is on sale with 40% off
    public class OnSaleOffPromotion : BaseProductPromotion
    {
        public override string Desc { get { return "Selected products are on sale with certain percent off"; } }

        // slightly different naming convention
        protected new class RuleModel : BaseRuleModel
        {
            public int off { get; set; }       // on sell with .off% off against regular price
        }
        protected new RuleModel RuleObj;

        protected override void ValidateRule(string rule) 
        {
            int offRatio;

            var good = int.TryParse(rule, out offRatio);
            if (!good || offRatio <= 0)
                throw new InvalidPromotionRuleException("OnSaleOff Promotion: int(>0) expected");
        }

        protected override void ParseRule() 
        {
            this.RuleObj = new RuleModel() { off = int.Parse(this.Rule) };
        }

        public override string ToString()
        {
            return string.Format("On Sale At {0}% Off", this.RuleObj.off);
        }

        // Calculate the total price to be paid
        protected override decimal Execute()
        {
            return this.context.Quantity * this.context.Price * (100 - this.RuleObj.off) / 100;       
        }
    }
}
