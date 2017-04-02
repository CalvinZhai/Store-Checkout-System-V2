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
    [TestFixture(Description = "We'll use APIs completely to test order level promotion than loading data from Xml files")]
    public class TestOrderLevelPromotionsByAPI
    {
        [Test]
        public void Test_CityEmploy_Additional_5PercentOff()
        {
            var order = new Order()
            {
                Customer = new CityEmploy()                
                {
                    Name = "Calvin Zhai",
                    Address = "123 4 Ave SW Calgary"
                },

                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 10, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.00), 
                        }
                    }
                },

                Promotions = new List<IPromotion>()
                {
                    new VIPOffPromotion() { Rule = "10" },
                    new CityEmployeePromotion() { Rule = "5" },
                    new OnSalePricedPromotion() { Rule = "0.5", Products = new List<string>() {"*"} },     // 5.00 
                }
            };

            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(10 * 1.00 / 2 * (1 - 0.05)));
            Assert.AreEqual(order.AppliedPromotions.Count, 1);
            Assert.AreEqual(order.AppliedPromotions.First().GetType(), typeof(CityEmployeePromotion));
        }

        [Test]
        public void Test_VIP_Additional_10PercentOff()
        {
            var order = new Order()
            {
                Customer = new VIPCustomer()                
                {
                    Name = "Calvin Zhai",
                    Address = "123 4 Ave SW Calgary"
                },

                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 10, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.00), 
                        }
                    }
                },

                Promotions = new List<IPromotion>()
                {
                    new VIPOffPromotion() { Rule = "10" },
                    new CityEmployeePromotion() { Rule = "5" },
                    new OnSalePricedPromotion() { Rule = "0.5", Products = new List<string>() {"*"} },     // 5.00 
                }
            };

            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(10 * 1.00 / 2 * (1 - 0.10)));
            Assert.AreEqual(order.AppliedPromotions.Count, 1);
            Assert.AreEqual(order.AppliedPromotions.First().GetType(), typeof(VIPOffPromotion));
        }

        [Test]
        public void Test_VIP_CityEmployee_Additional_15PercentOff()
        {
            var order = new Order()
            {
                Customer = new VIPCityEmploy()                
                {
                    Name = "Calvin Zhai",
                    Address = "123 4 Ave SW Calgary"
                },

                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 10, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.00), 
                        }
                    }
                },

                Promotions = new List<IPromotion>()
                {
                    new VIPOffPromotion() { Rule = "10" },
                    new CityEmployeePromotion() { Rule = "5" },
                    new OnSalePricedPromotion() { Rule = "0.5", Products = new List<string>() {"*"} },     // 5.00 
                }
            };

            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(10 * 1.00 / 2 * (1 - 0.10) * (1 - 0.05)));
            Assert.AreEqual(order.AppliedPromotions.Count, 2);
            Assert.IsTrue(order.AppliedPromotions.Exists(x=> x.GetType().Name == typeof(VIPOffPromotion).Name));
            Assert.IsTrue(order.AppliedPromotions.Exists(x => x.GetType().Name == typeof(CityEmployeePromotion).Name));
        }
    }
}
