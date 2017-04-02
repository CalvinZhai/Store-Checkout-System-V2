using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class VIPOffPromotion : BaseOrderPromotion
    {
        public override string Desc { get { return "VIP customer will get an additional % off"; } }

        // slightly different naming convention
        protected new class RuleModel : BaseRuleModel
        {
            public int off { get; set; }            // additional .off% off against total selling price
        }
        protected new RuleModel RuleObj;

        protected override void ValidateRule(string rule)
        {
            int offRatio;

            var good = int.TryParse(rule, out offRatio);
            if (!good || offRatio <= 0)
                throw new InvalidPromotionRuleException("VIPOff Promotion: int(>0) expected");
        }

        protected override void ParseRule()
        {
            this.RuleObj = new RuleModel() { off = int.Parse(this.Rule) };
        }

        public override string ToString()
        {
            return string.Format("VIP At Additional {0}% Off", this.RuleObj.off);
        }

        protected override bool Applicable()
        { return this.context.IsVIP; }

        // Calculate the total price to be paid
        protected override decimal Execute()
        {
            return this.context.TotalSellingPrice * (100 - this.RuleObj.off) / 100;
        }
    }
}
