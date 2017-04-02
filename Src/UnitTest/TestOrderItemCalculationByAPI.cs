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
    // will also test "*" and "*-" wild cards for Promotion.Products
    [TestFixture(Description = "We'll use APIs completely to test order item level calculation than loading data from Xml files")]
    public class TestOrderItemCalculationByAPI
    {
        [Test(Description = "test to ensure '*' will have promotion applying to any product")]
        public void Test_Promotion_Product_Asterisk_Qualifier()
        {
            var item = new OrderItem() 
            { 
                Quantity = 10, 
                Product = new Product() 
                { 
                    Name = "Apple", 
                    Price = new decimal(1.20), 
                }
            };

            var totalSellingPrice = item.Calculate(new List<IProductPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "1.0", Products = new List<string>() {"*"} }    // $10.0
                }
            );

            Assert.AreEqual(totalSellingPrice, new decimal(10.0));
        }

        [Test(Description = "test to ensure '*-' will have promotion applying to any product except...")]
        public void Test_Promotion_Product_Asterisk_Minus_Qualifier_1()
        {
            var item = new OrderItem()
            {
                Quantity = 10,
                Product = new Product()
                {
                    Name = "Apple",
                    Price = new decimal(1.20),
                }
            };

            var totalSellingPrice = item.Calculate(new List<IProductPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "1.0", Products = new List<string>() {"*- apple, banana"} }    // $10.0
                }
            );

            Assert.AreEqual(totalSellingPrice, new decimal(12.0));
            Assert.AreEqual(item.AppliedPromotion, null);           // not applied
        }

        [Test(Description = "test to ensure '*-' will have promotion applying to any product except...")]
        public void Test_Promotion_Product_Asterisk_Minus_Qualifier_2()
        {
            var item = new OrderItem()
            {
                Quantity = 10,
                Product = new Product()
                {
                    Name = "baNAna",
                    Price = new decimal(1.20),
                }
            };

            var totalSellingPrice = item.Calculate(new List<IProductPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "1.0", Products = new List<string>() {"*- apple, banana"} }    // $10.0
                }
            );

            Assert.AreEqual(totalSellingPrice, new decimal(12.0));
            Assert.AreEqual(item.AppliedPromotion, null);               // not applied
        }

        [Test(Description = "test to ensure '*-' will have promotion applying to any product except...")]
        public void Test_Promotion_Product_Asterisk_Minus_Qualifier_3()
        {
            var item = new OrderItem()
            {
                Quantity = 10,
                Product = new Product()
                {
                    Name = "OrAnGe",
                    Price = new decimal(1.20),
                }
            };

            var totalSellingPrice = item.Calculate(new List<IProductPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "1.0", Products = new List<string>() {"*- apple, banana"} }    // $10.0
                }
            );

            Assert.AreEqual(totalSellingPrice, new decimal(10));
            Assert.AreEqual(item.AppliedPromotion.GetType(), typeof(OnSalePricedPromotion));        // applied
        }

        [Test(Description = "Buy 14 Apple (regular price = $1.2) against Promotion 'Buy 3 get 2 free' and 'Buy 3 get 2 for 40% off'")]
        public void Test_14Apple_Reglar_1Dot4_Buy3Get4For40PercentOff()
        {
            var item = new OrderItem() 
            { 
                Quantity = 14, 
                Product = new Product() 
                { 
                    Name = "Apple", 
                    Price = new decimal(1.20), 
                }
            };

            var totalSellingPrice = item.Calculate(new List<IProductPromotion>()
                {
                    new GroupAdditionOffPromotion() { Rule = "3-2-40", Products = new List<string>() {"*"} }    // $14.4
                }
            );

            Assert.AreEqual(totalSellingPrice, new decimal(14.4));
            Assert.AreEqual(item.TotalSellingPrice, new decimal(14.4));
        }

        [Test(Description = "Buy 14 Apple (regular price = $1.2) against all 4 promotions")]
        public void Test_14Apple_Regular_1Dot2_With_All_4_Promotions()
        {
            var item = new OrderItem()
            {
                Quantity = 14,
                Product = new Product()
                {
                    Name = "Apple",
                    Price = new decimal(1.20),
                }
            };

            var totalSellingPrice = item.Calculate(new List<IProductPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "0.8",         Products = new List<string>() {"*"} },      // $11.2
                    new GroupPricedPromotion()  { Rule = "3-2.0" ,      Products = new List<string>() {"*"}},       // $10.4
                    new GroupAdditionFreePromotion() { Rule = "3-2" ,   Products = new List<string>() {"*"}},       // $10.8
                    new GroupAdditionOffPromotion()  { Rule = "3-2-40", Products = new List<string>() {"*"} }       // $14.4
                }
            );

            Assert.AreEqual(totalSellingPrice, new decimal(10.4));
            Assert.AreEqual(item.TotalSellingPrice, new decimal(10.4));
            Assert.AreEqual(item.AppliedPromotion.GetType().Name, typeof(GroupPricedPromotion).Name);
        }
    }
}
