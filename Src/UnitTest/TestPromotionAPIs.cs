using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using GroceryCo.Checkout;
using GroceryCo.Checkout.Framework;
using GroceryCo.Checkout.Domain;
using GroceryCo.Checkout.Client;

namespace GroceryCo.Checkout.UnitTest
{
    [TestFixture(Description = "Test all Promotion implementations' APIs")]
    public class TestPromotionAPIs : TestBase
    {
        [ExpectedException(typeof(PromotionRuleEmptyException))]
        [Test(Description="")]
        public void ExpectException_GroupAdditionOffPromotion_EmptyRule()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "" };
        }

        [ExpectedException(typeof(InvalidPromotionRuleException))]
        [Test(Description = "")]
        public void ExpectException_GroupAdditionOffPromotion_ZeroBuyFactor()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "0" };
        }

        [ExpectedException(typeof(InvalidPromotionRuleException))]
        [Test(Description = "")]
        public void ExpectException_GroupAdditionOffPromotion_ZeroAddFactor()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3-0" };
        }

        [ExpectedException(typeof(InvalidPromotionRuleException))]
        [Test(Description = "")]
        public void ExpectException_GroupAdditionOffPromotion_ZeroOffFactor()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3-2-0" };
        }

        [ExpectedException(typeof(InvalidPromotionRuleException))]
        [Test(Description = "")]
        public void ExpectException_GroupAdditionOffPromotion_LessThan3Factors()
        {
            var promotion = new GroupAdditionOffPromotion() { Rule = "3-2" };
        }

        [Test]
        public void Test_OnSalePricedPromotionAPIs()
        {
            var promotion = new OnSalePricedPromotion()
            {
                Rule = ".8",
                Products = new List<string>() { "*" }
            };
            var calculatedPrice = promotion.Apply(new BaseProductPromotionContext() 
            { 
                Product = "apple",
                Price = new decimal(1.0),
                Quantity = 10
            });

            Assert.AreEqual(calculatedPrice, new decimal(.8 * 10));
        }

        [Test]
        public void Test_OnSaleOffPromotionAPIs()
        {
            var promotion = new OnSaleOffPromotion()
            {
                Rule = "40",
                Products = new List<string>() { "*" }
            };
            var calculatedPrice = promotion.Apply(new BaseProductPromotionContext()
            {
                Product = "apple",
                Price = new decimal(1.0),
                Quantity = 2
            });

            Assert.AreEqual(calculatedPrice, new decimal(2 * 1 * (1-0.4)));
        }

        [Test]
        public void Test_GroupPricedPromotionAPIs()
        {
            var promotion = new GroupPricedPromotion()
            {
                Rule = "3-2.0",
                Products = new List<string>() { "*" }
            };
            var calculatedPrice = promotion.Apply(new BaseProductPromotionContext()
            {
                Product = "apple",
                Price = new decimal(1.2),
                Quantity = 7
            });

            Assert.AreEqual(calculatedPrice, new decimal(7 / 3 * 2.0 + 7 % 3 * 1.2));
        }

        [Test]
        public void Test_GroupAdditionOffPromotionAPIs()
        {
            var promotion = new GroupAdditionOffPromotion()
            {
                Rule = "3-2-40",
                Products = new List<string>() { "*" }
            };
            var calculatedPrice = promotion.Apply(new BaseProductPromotionContext()
            {
                Product = "apple",
                Price = new decimal(1.2),
                Quantity = 14
            });

            Assert.AreEqual(calculatedPrice,
                        new decimal
                        (
                                (14 / (3 + 2)) * (3 * 1.2)
                                +
                                (14 / (3 + 2)) * (2 * 1.2 * (1 - 0.4))
                                +
                                3 * 1.2
                                +
                                (14 % (3 + 2) - 3) * 1.2 * (1 - 0.4)
                        ) 
            );
        }
    }
}
