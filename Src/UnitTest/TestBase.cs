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
    // // Directory.GetCurrentDirectory(): E:\ZhaiPW\App\Absorb Kiosk Checkout\Src\UnitTest\bin\Debug\
    [TestFixture]
    public class TestBase
    {
        // Test data folder
        public string DataFolder
        {
            get { return Directory.GetCurrentDirectory() + @"\..\..\Data\"; }
        }

        [SetUp]
        public void Init()
        {
            
        }

        [TearDown]
        public void Clean()
        {
            
        }
    }
}
