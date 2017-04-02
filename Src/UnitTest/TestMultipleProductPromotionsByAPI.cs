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
    [TestFixture(Description="We'll use APIs completely to unit test than loading data from Xml files")]
    public class TestMultipleProductPromotionsByAPI : TestBase
    {
        [Test(Description = "Handy method helping quickly get calculated order total selling price " + 
                            "against a single promotion by commenting/uncommenting out promotions block")]
        public void Test_Get_Total_Selling_Price()
        {
            var order = new Order()
            {
                Customer = new Customer()
                {
                    Name = "Calvin Zhai",
                    Address = "123 4 Ave SW Calgary"
                },
                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 14, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.20), 
                        }
                    }
                },
                Promotions = new List<IPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "0.8",         Products = new List<string>() {"*"} }       // $11.2
                    //new GroupPricedPromotion()  { Rule = "3-2.0",       Products = new List<string>() {"*"} }       // $10.4
                    //new GroupAdditionFreePromotion() { Rule = "3-2",    Products = new List<string>() {"*"} }       // $10.8
                    //new GroupAdditionOffPromotion()  { Rule = "3-2-40", Products = new List<string>() {"*"} }       // $14.4
                }
            };

            order.Calculate();

            Console.WriteLine(string.Format("Order total price to be paid: ${0}", order.TotalSellingPrice));
        }

        [Test(Description = "Buy 14 Apple (regular price = $1.2) against Promotion 'On sale at $0.8' and 'Buy 3 for $2.0'")]
        public void Test_14Apple_Regular_1Dot2_OnSaleAtZeroDot8_And_Buy3For2Dollars()
        {
            var order = new Order() 
            { 
                Customer = new Customer() 
                { 
                    Name = "Calvin Zhai", Address = "123 4 Ave SW Calgary" 
                },
                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 14, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.20), 
                        }
                    }
                },
                Promotions = new List<IPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "0.8",   Products = new List<string>() {"*"} },
                    new GroupPricedPromotion()  { Rule = "3-2.0", Products = new List<string>() {"*"} }
                }
            };

            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, Math.Min(new decimal( 14 * 0.8 ), new decimal(14 / 3 * 2.0 + 14 % 3 *1.2) ));
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(GroupPricedPromotion).Name);

        }

        [Test(Description = "Buy 14 Apple (regular price = $1.2) against Promotion 'Buy 3 get 2 free' and 'Buy 3 get 2 for 40% off'")]
        public void Test_14Apple_Regular_1Dot2_Buy3Get2Free_And_Buy3Get2For40PercentOff()
        {
            var order = new Order()
            {
                Customer = new Customer()
                {
                    Name = "Calvin Zhai",
                    Address = "123 4 Ave SW Calgary"
                },
                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 14, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.20), 
                        }
                    }
                },
                Promotions = new List<IPromotion>()
                {
                    new GroupAdditionFreePromotion() { Rule = "3-2",    Products = new List<string>() {"*"} },
                    new GroupAdditionOffPromotion()  { Rule = "3-2-40", Products = new List<string>() {"*"} }
                }
            };

            order.Calculate();

            Assert.AreEqual(
                                order.TotalSellingPrice, 
                                Math.Min(
                                    new decimal
                                    (
                                        (14 / (3 + 2)) * (3 * 1.2) + 3 * 1.2
                                    ), 
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
                                )
            );
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(GroupAdditionFreePromotion).Name);

        }

        [Test(Description = "Buy 14 Apple (regular price = $1.2) against all 4 promotions")]
        public void Test_14Apple_Regular_1Dot2_With_All_4_Promotions()
        {
            var order = new Order()
            {
                Customer = new Customer()
                {
                    Name = "Calvin Zhai",
                    Address = "123 4 Ave SW Calgary"
                },
                Items = new List<OrderItem>() 
                { 
                    new OrderItem() 
                    { 
                        Quantity = 14, 
                        Product = new Product() 
                        { 
                            Name = "Apple", 
                            Price = new decimal(1.20), 
                        }
                    }
                },
                Promotions = new List<IPromotion>()
                {
                    new OnSalePricedPromotion() { Rule = "0.8",         Products = new List<string>() {"*"} },      // $11.2
                    new GroupPricedPromotion()  { Rule = "3-2.0" ,      Products = new List<string>() {"*"}},       // $10.4
                    new GroupAdditionFreePromotion() { Rule = "3-2" ,   Products = new List<string>() {"*"}},       // $10.8
                    new GroupAdditionOffPromotion()  { Rule = "3-2-40", Products = new List<string>() {"*"} }       // $14.4
                }
            };

            order.Calculate();

            Assert.AreEqual(order.TotalSellingPrice, new decimal(10.4));
            Assert.AreEqual(order.Items.First().AppliedPromotion.GetType().Name, typeof(GroupPricedPromotion).Name);
        }
    }
}
