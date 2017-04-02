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
    public class TestOnSaleOffPromotion : TestBase
    {
        [Test(Description = "2 Apple (regular price = $1.0) on sale at 40% off")]
        public void Test_2Apple_OnSaleAt_40Percent_Off()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"OnSaleOffPromotion\Basket.xml",
                                               this.DataFolder + @"OnSaleOffPromotion\ProductCatalog.xml");
            var order = builder.Build();
            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(2 * 1 * (1-0.4)));
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(OnSaleOffPromotion).Name);

        }
    }
}
