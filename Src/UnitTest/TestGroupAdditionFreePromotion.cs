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
    public class TestGroupAdditionFreePromotion : TestBase
    {
        [Test(Description = "Buy 14 Apple (regular price = $1.2) against Promotion 'Buy 3 get 2 free'")]
        public void Test_14Apple_Regular_1Dot2_Buy3Get2Free()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"GroupAdditionFreePromotion\Basket.xml",
                                               this.DataFolder + @"GroupAdditionFreePromotion\ProductCatalog.xml");
            var order = builder.Build();
            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal( (14 / (3 + 2)) * (3 * 1.2) + 3 * 1.2 ) );
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(GroupAdditionFreePromotion).Name);

        }
    }
}
