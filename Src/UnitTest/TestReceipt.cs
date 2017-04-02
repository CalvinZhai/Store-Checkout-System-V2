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
    [TestFixture(Description = "We'll use APIs completely to test receipt than loading data from Xml files")]
    public class TestReceipt : TestBase
    {
        [Test]
        public void Test_Receipt_14Apple_Buy3Get4For40PercentOff()
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
                    new GroupAdditionOffPromotion() { Rule = "3-2-40", Products = new List<string>() {"*"} }    // $14.4
                }
            };

            order.Calculate();

            Assert.AreEqual(order.Receipt.TotalSellingPrice, new decimal(14.4));
        }
    }
}
