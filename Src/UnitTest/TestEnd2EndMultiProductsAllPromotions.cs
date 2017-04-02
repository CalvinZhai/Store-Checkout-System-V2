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
    public class TestEnd2EndMultiProductsAllPromotions : TestBase
    {
        // Apple:   4, regular price $1.0, 2 promotions: on-sale at $0.8, and on-sale at 40% off
        // Banana: 14, regular price $2.0, 2 promotions: buy 3 at $4.10, and buy 3 get 2 for free
        // Orange: 14, regular price $3.0, 2 promotions: buy 3 get 2 for free, and buy 2 get 3 with 50% off
        [Test(Description = "Throughout test...")]
        public void End2End_MultiProducts_AllPromotions()
        {
            OrderItem item;
            var builder = new OrderFileBuilder(this.DataFolder + @"End2EndMultiProductsAllPromotions\Normal-Basket.xml",
                                               this.DataFolder + @"End2EndMultiProductsAllPromotions\Normal-ProductCatalog.xml");
            var order = builder.Build();
            order.Calculate();

            Assert.AreEqual(order.Items.Count, 3);

            Console.WriteLine(" ---- Apple ----");
            item = order.Items.Where(x => x.Product.Name.ToUpper() == "apple".ToUpper()).First();
            Assert.AreEqual(item.TotalSellingPrice, new decimal(2.4));      // vs. 3.2
            Assert.AreEqual(item.AppliedPromotion.GetType(), typeof(OnSaleOffPromotion));

            Console.WriteLine(" ---- Banana ----");
            item = order.Items.Where(x => x.Product.Name.ToUpper() == "BaNaNa".ToUpper()).First();
            Assert.AreEqual(item.TotalSellingPrice, new decimal(18));       // vs 18.4  
            Assert.AreEqual(item.AppliedPromotion.GetType(), typeof(GroupAdditionFreePromotion));

            Console.WriteLine(" ---- Orange ----");
            item = order.Items.Where(x => x.Product.Name.ToUpper() == "OranGe".ToUpper()).First();
            Assert.AreEqual(item.TotalSellingPrice, new decimal(27.0));     // vs 30.0
            Assert.AreEqual(item.AppliedPromotion.GetType(), typeof(GroupAdditionFreePromotion));
        }

    }
}
