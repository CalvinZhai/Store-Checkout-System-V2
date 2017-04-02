using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using NUnit.Framework;

using GroceryCo.Checkout;
using GroceryCo.Checkout.Framework;
using GroceryCo.Checkout.Domain;
using GroceryCo.Checkout.Client;

namespace GroceryCo.Checkout.UnitTest
{
    [TestFixture]
    public class TestPromotionEssentials : TestBase
    {
        [Test(Description = "Expect exception if promotion .Rule is empty")]
        [ExpectedException(typeof(PromotionRuleEmptyException))]
        public void ExpectException_If_PromotionRuleEmpty()
        {
            var promotion = new OnSalePricedPromotion();
            promotion.Rule = "";
        }

        [Test(Description = "Expect exception when creating promotion instance which is not implemented yet")]
        [ExpectedException(typeof(PromotionNotImplementedException))]
        public void ExpectException_When_Create_NonExistPromotionInstance()
        {
            var promotion = BaseProductPromotion.Create("non-exist-promotion-name-anyway");
        }

        #region OnsalePricedPromotion cases

        [Test(Description = "Shall go well")]
        public void OnsalePricedPromotion_ValidRule()
        {
            var promotion = new OnSalePricedPromotion() { Rule = "2.0" };
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void OnsalePricedPromotion_ExpectException_OnInvalidRule()
        {
            var promotion = new OnSalePricedPromotion() { Rule = "-2.0" };      // negative is invalid
        }

        #endregion

        #region GroupPricedPromotion cases

        [Test(Description = "Shall go well")]
        public void GroupPricedPromotion_ValidRule()
        {
            var promotion = new GroupPricedPromotion() { Rule = "3-2.0" };     // buy 3 for $2.0
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupPricedPromotion_ExpectException_OnInvalidRule_1()
        {
            var promotion = new GroupPricedPromotion() { Rule = "3" };      
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupPricedPromotion_ExpectException_OnInvalidRule_2()
        {
            var promotion = new GroupPricedPromotion() { Rule = "3-" };      
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupPricedPromotion_ExpectException_OnInvalidRule_3()
        {
            var promotion = new GroupPricedPromotion() { Rule = "3-0" };      // 0 is invalid
        }

        #endregion

        #region GroupAdditionOffPromotion cases

        [Test(Description = "Shall go well")]
        public void GroupAdditionOffPromotion_ValidRule()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3-2-40" };     // buy 3 get 2 for 40% free
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupAdditionOffPromotion_ExpectException_OnInvalidRule_1()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3" };
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupAdditionOffPromotion_ExpectException_OnInvalidRule_2()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3-2" };
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupAdditionOffPromotion_ExpectException_OnInvalidRule_3()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3-2-0" };      // 0 is invalid
        }

        #endregion

        #region GroupAdditionFreePromotion cases

        [Test(Description = "Shall go well")]
        public void GroupAdditionFreePromotion_ValidRule()
        {
            var promotion = new GroupPricedPromotion() { Rule = "3-2" };     // buy 3 get 2 for free
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupAdditionFreePromotion_ExpectException_OnInvalidRule_1()
        {
            var promotion = new GroupAdditionFreePromotion() { Rule = "3" };      
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupAdditionFreePromotion_ExpectException_OnInvalidRule_2()
        {
            var promotion = new GroupAdditionFreePromotion() { Rule = "3-" };      
        }

        [Test]
        [ExpectedException(typeof(InvalidPromotionRuleException))]
        public void GroupAdditionFreePromotion_ExpectException_OnInvalidRule_3()
        {
            var promotion = new GroupAdditionFreePromotion() { Rule = "3-0" };      // 0 is invalid
        }

        #endregion

    }
}
