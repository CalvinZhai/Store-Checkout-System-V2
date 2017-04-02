using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Configuration;
using System.IO;

using GroceryCo.Checkout.Framework;
using GroceryCo.Checkout.Domain;

namespace GroceryCo.Checkout.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new OrderFileBuilder(ConfigurationManager.AppSettings["BasketUri"] as string,
                                                   ConfigurationManager.AppSettings["ProductCatalogProxyUri"] as string);
                var order = builder.Build();
                var totalSellingPrice = order.Calculate();

                Console.WriteLine("------------------- Receipt -------------------------");
                Print2File(order.Receipt);
                Print2Console(order.Receipt);
                Console.WriteLine();
            }
            catch (BaseException ex)
            {
                Console.WriteLine(".......Handled Exception, Invalid Data......");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Exception Type:" + ex.GetType().Name);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(".......Unhandled Exception, likely BAD data in xml files......");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);
            }

            Console.Read();
        }

        // let's print to Console and OrderReceipt.Output.txt directly
        static void Print2File(Receipt receipt)
        {
            // Direct Console output to external file
            // System.IO.Directory.GetCurrentDirectory(): E:\ZhaiPW\App\Absorb Kiosk Checkout\Src\Client\bin\Debug\
            string outputFile = Path.Combine(Directory.GetCurrentDirectory(), "OrderReceipt.Output.txt");
            var writer = new StreamWriter(outputFile);
            writer.AutoFlush = true;
            Console.SetOut(writer);

            Print2Console(receipt);

            // Direct Console output back to normal
            writer = new StreamWriter(Console.OpenStandardOutput());
            writer.AutoFlush = true;
            Console.SetOut(writer);
            Console.WriteLine(string.Format("Output file generated: {0}", outputFile));
            Console.WriteLine();
        }

        static void Print2Console(Receipt receipt)
        {
            // Customer
            Console.WriteLine(string.Format("Customer Name: {0}", receipt.Customer.Name));
            Console.WriteLine(string.Format("Customer Address: {0}", receipt.Customer.Address));
            Console.WriteLine();

            // Order
            Console.WriteLine(string.Format("Purchase Date: {0}", receipt.CreatedOn));
            Console.WriteLine(string.Format("Total Regular Price: {0}", receipt.TotalRegularPrice));
            Console.WriteLine(string.Format("Total Price To Be Paid: {0}", receipt.TotalSellingPrice));
            Console.WriteLine(string.Format("Total Savings: {0}", receipt.TotalSavings));
            receipt.Promotions.ForEach(x => Console.WriteLine(string.Format("Order-Level Promotion: {0}",x)));
            Console.WriteLine();

            // Order Items 
            foreach (var item in receipt.Items)
            {
                Console.WriteLine(string.Format("Product: {0}", item.ProductName));
                Console.WriteLine(string.Format("Quantity: {0}", item.Quantity));
                Console.WriteLine(string.Format("Regular Price: {0}", item.RegularPrice));
                Console.WriteLine(string.Format("Total Regular Price: {0}", item.TotalRegularPrice));
                Console.WriteLine(string.Format("Total Price To Be Paid: {0}", item.TotalSellingPrice));
                Console.WriteLine(string.Format("Total Savings: {0}", item.TotalSavings));
                if (item.Promotion != "")
                    Console.WriteLine(string.Format("Promotion Applied: {0}", item.Promotion));
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
