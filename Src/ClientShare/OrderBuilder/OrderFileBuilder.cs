/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using GroceryCo.Checkout.Domain;

namespace GroceryCo.Checkout.Client
{
    public class OrderFileBuilder : DefaultOrderBuilder
    {
        private string basketFile = "";                 // client basket file path
        private string catalogFile = "";                // product catalog file path

        private ICatalogProvider catalogProvider;

        public OrderFileBuilder(string basketFilePath, string productCatalogFilePath)
        {
            this.basketFile = basketFilePath;
            this.catalogFile = productCatalogFilePath;

            // will load products' price, promotion data from file 
            catalogProvider = new ProductCatalogFileProxy(productCatalogFilePath);  
        }

        public Order Build()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(this.basketFile);

            var order = this.Build(this.ParseProducts(xmlDoc), this.catalogProvider.Promotions, this.ParseCustomer(xmlDoc));
            return order;
        }

        // <Customer> is optional
        private Customer ParseCustomer(XmlDocument xmlDoc)
        {
            var customer = new Customer();

            var xmlNodes = xmlDoc.DocumentElement.SelectNodes("/Basket/Customer");
            if (xmlNodes.Count > 0)
            {
                var xmlNode = xmlNodes[0];

                if (xmlNode["Name"] != null)      // <Name> optional
                    customer.Name = xmlNode["Name"].InnerText.Trim(); 

                if (xmlNode["Address"] != null)   // <Address> optional
                    customer.Address = xmlNode["Address"].InnerText.Trim();

            }

            return customer;
        }

        private List<Product> ParseProducts(XmlDocument xmlDoc)
        {
            var products = new List<Product>();

            var xmlNodes = xmlDoc.DocumentElement.SelectNodes("/Basket/Items/Item");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                if (xmlNode["Name"] != null)
                {
                    string productName = xmlNode["Name"].InnerText.Trim().ToUpper();

                    // .GetProduct will throw exception when no matching product being found
                    var product = this.catalogProvider.GetProduct(productName);     

                    products.Add(product);
                }
                else                  // product name is mandatory
                    throw new ProductNameNodeMissedException("OrderFileBuilder: <Item>'s <Name> subnode missed");
            }

            return products;
        }
    }
}
