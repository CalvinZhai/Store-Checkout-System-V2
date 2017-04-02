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
    public class TestGroupPricedPromotion : TestBase
    {
        [Test(Description = "Buy 7 Apple (regular price = $1.2) against Promotion 'Buy 3 at $2.00'")]
        public void Test_7Apple_Regular_1Dot2_Buy3At2()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"GroupPricedPromotion\Basket.xml",
                                               this.DataFolder + @"GroupPricedPromotion\ProductCatalog.xml");
            var order = builder.Build();
            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(7 / 3 * 2.0 + 7 % 3 * 1.2));
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(GroupPricedPromotion).Name);

        }
    }
}
