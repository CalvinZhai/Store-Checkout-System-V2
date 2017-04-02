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
    public class TestProductCatalogFileProxy : TestBase
    {
        [Test(Description = "")]
        public void Normal_Lood()
        {
            var proxy = new ProductCatalogFileProxy();
            proxy.Load(this.DataFolder + @"ProductCatalogFileProxy\Normal-ProductCatalog.xml");

            Assert.AreEqual(proxy.Products.Count, 3);
            Assert.IsTrue(proxy.Products.ContainsKey("apple".ToUpper()));
            Assert.IsTrue(proxy.Products.ContainsKey("Banana".ToUpper()));
            Assert.IsTrue(proxy.Products.ContainsKey("OrAnGE".ToUpper()));

            Assert.AreEqual(proxy.Promotions.Count, 5);

            // OnSalePriced
            var promotion = proxy.Promotions.Where(x => x.GetType() == typeof(OnSalePricedPromotion)).Cast<IProductPromotion>().First();
            Assert.AreEqual(promotion.Rule, ".8");
            Assert.AreEqual(promotion.Products.Count, 1);
            Assert.AreEqual(promotion.Products[0], "*");

            // OnSaleOff
            promotion = proxy.Promotions.Where(x => x.GetType() == typeof(OnSaleOffPromotion)).Cast<IProductPromotion>().First();
            Assert.AreEqual(promotion.Rule, "40");
            Assert.AreEqual(promotion.Products.Count, 1);
            Assert.AreEqual(promotion.Products[0], "Apple");

            // GroupPriced
            promotion = proxy.Promotions.Where(x => x.GetType() == typeof(GroupPricedPromotion)).Cast<IProductPromotion>().First();
            Assert.AreEqual(promotion.Rule, "3-2.0");
            Assert.AreEqual(promotion.Products.Count, 2);
            Assert.IsTrue(promotion.Products.Exists(x => x.ToUpper() == "apple".ToUpper()));
            Assert.IsTrue(promotion.Products.Exists(x => x.ToUpper() == "bAnAna".ToUpper()));
            Assert.IsFalse(promotion.Products.Exists(x => x.ToUpper() == "fdafds".ToUpper()));

            // GroupAdditionFree
            promotion = proxy.Promotions.Where(x => x.GetType() == typeof(GroupAdditionFreePromotion)).Cast<IProductPromotion>().First();
            Assert.AreEqual(promotion.Rule, "3-2-100");
            Assert.AreEqual(promotion.Products.Count, 2);
            Assert.IsTrue(promotion.Products.Exists(x => x.ToUpper() == "Banana".ToUpper()));
            Assert.IsTrue(promotion.Products.Exists(x => x.ToUpper() == "Orange".ToUpper()));
            Assert.IsFalse(promotion.Products.Exists(x => x.ToUpper() == "apple".ToUpper()));

            // GroupAdditionOff
            promotion = proxy.Promotions.Where(x => x.GetType() == typeof(GroupAdditionOffPromotion)).Cast<IProductPromotion>().First();
            Assert.AreEqual(promotion.Rule, "3-2-50");
            Assert.AreEqual(promotion.Products.Count, 1);
            Assert.IsTrue(promotion.Products.Exists(x => x.ToUpper() == "orange".ToUpper()));
        }

        [Test(Description = "Expect exception if catalog product name is empty")]
        [ExpectedException(typeof(ProductNameEmptyException))]
        public void ExpectException_If_Catalog_ProductName_Empty()
        {
            var proxy = new ProductCatalogFileProxy();
            proxy.Load(this.DataFolder + @"ProductCatalogFileProxy\ProductNameEmpty-ProductCatalog.xml");
            
        }

        [Test(Description = "Expect exception if having duplicated product in catalog")]
        [ExpectedException(typeof(DuplicatedProductException))]
        public void ExpectException_If_Catalog_ProductName_Duplicated()
        {
            var proxy = new ProductCatalogFileProxy();
            proxy.Load(this.DataFolder + @"ProductCatalogFileProxy\DuplicatedProduct-ProductCatalog.xml");
        }

        [Test(Description = "Expect exception if basket product not exist in catalog")]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void ExpectException_If_Catalog_Product_NotExist()
        {
            var proxy = new ProductCatalogFileProxy();
            proxy.Load(this.DataFolder + @"ProductCatalogFileProxy\Normal-ProductCatalog.xml");

            proxy.GetProduct("never-exist-product, will throw exception");
        }

    }
}
