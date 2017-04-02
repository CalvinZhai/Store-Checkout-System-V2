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
    public abstract class BasePromotion : BaseDomainObject, IPromotion
    {
        // Used to cache IProductPromotion Types: key: promotion name, value: promotion type
        private static Dictionary<string, Type> promotionTypes = new Dictionary<string, Type>();

        static BasePromotion()
        {
            var type = typeof(IPromotion);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            promotionTypes = types.ToDictionary<System.Type, string>(x => x.Name);
        }

        // Function method to create a IProductPromotion instance based on passed-in 'promotionName'
        public static IPromotion Create(string promotionName)
        {
            var pn = promotionName.Trim() + "Promotion";
            var typeKV = promotionTypes.Where(x => x.Key == pn);

            if (typeKV.Count() > 0)
            {
                var promotion = (IPromotion)Activator.CreateInstance(typeKV.First().Value);

                return promotion;
            }
            else
            {
                throw new PromotionNotImplementedException(string.Format("BasePromotion.Create: {0}Promotion not implemented", promotionName));
            }
        }

        #region implement IPromotion

        public virtual string Desc { get { return ""; } }

        private string rule = "";
        public virtual string Rule
        {
            get { return this.rule; }
            set
            {
                var tv = value.Trim();

                if (tv == "")
                    throw new PromotionRuleEmptyException(string.Format("{0}Promotion Rule value cannot be empty", this.GetType().Name));
                else
                {
                    this.ValidateRule(tv);
                    this.rule = tv;
                    this.ParseRule();
                }
            }
        }

        // kind place holders..., will be override by sub-classes using 'new' keyword
        protected class RuleModel : BaseRuleModel { }
        protected RuleModel RuleObj;

        protected virtual void ValidateRule(string rule) { }    // Validate Rule string before parsing
        protected virtual void ParseRule() { }                  // Parse .Rule string into Rule Model object

        protected virtual bool Applicable()
        { return false; }

        protected abstract IPromotionContext Context { set; }   // will be used by .Execute
        public virtual decimal Apply(IPromotionContext promotionContext)
        {
            this.Context = promotionContext;

            if (this.Applicable())    // applicable
            {
                return this.Execute();
            }
            else                                     // not applicable
                return decimal.MinusOne;
        }

        // Execute this promotion, by default, just return decimal.MinusOne meaning not applicable
        // When NOT applicable, Order .Calculate() would use original total price as the finalized to be paid
        protected virtual decimal Execute()
        { return decimal.MinusOne; }

        #endregion
    }
}
