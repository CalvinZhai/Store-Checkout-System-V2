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
    public class TestOnSalePricedPromotion : TestBase
    {
        [Test(Description = "2 Apple (regular price = $1.0) on sale at $0.8")]
        public void Test_2Apple_OnSaleAt_ZeroDot8()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"OnSalePricedPromotion\Basket.xml",
                                               this.DataFolder + @"OnSalePricedPromotion\ProductCatalog.xml");
            var order = builder.Build();
            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(2 * 0.8));
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(OnSalePricedPromotion).Name);

        }
    }
}
