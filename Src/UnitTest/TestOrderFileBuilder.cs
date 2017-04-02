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
    public class TestOrderFileBuilder : TestBase
    {
        [Test(Description = "Test basket xml file loader to ensure product name is case INSENSITIVE, which means 'apple'='Apple'='APPLE'")]
        public void Ensure_Basket_ProductName_CaseInsensitive()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"OrderFileBuilder\ProductNameInsensitive-Basket.xml",
                                               this.DataFolder + @"OrderFileBuilder\ProductNameInsensitive-ProductCatalog.xml");
            var order = builder.Build();

            Assert.AreEqual(order.Items.Count, 1);  
            Assert.AreEqual(order.Items.First().Product.Name.ToUpper(), "apple".ToUpper());
            Assert.AreEqual(order.Items.First().Quantity, 4);
        }

        [Test(Description = "Expect exception if basket product name is empty(so not matching found in catalog)")]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void ExpectException_If_Basket_ProductName_Empty()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"OrderFileBuilder\ProductNameIsEmpty-Basket.xml",
                                               this.DataFolder + @"OrderFileBuilder\Normal-ProductCatalog.xml");
            var order = builder.Build();
        }

        [Test(Description = "Expect exception if catalog product name is empty")]
        [ExpectedException(typeof(ProductNameEmptyException))]
        public void ExpectException_If_Catalog_ProductName_Empty()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"OrderFileBuilder\Normal-Basket.xml",
                                               this.DataFolder + @"OrderFileBuilder\ProductNameIsEmpty-ProductCatalog.xml");
            var order = builder.Build();
        }

        [Test(Description = "Expect exception if basket product not exist in catalog")]
        [ExpectedException(typeof(ProductNotFoundException))]
        public void ExpectException_If_Basket_Product_NotExistInCatalog()
        {
            var builder = new OrderFileBuilder(this.DataFolder + @"OrderFileBuilder\ProductNotExistInCatalog-Basket.xml",
                                               this.DataFolder + @"OrderFileBuilder\ProductNotExistInCatalog-ProductCatalog.xml");
            var order = builder.Build();
        }

        
    }
}
