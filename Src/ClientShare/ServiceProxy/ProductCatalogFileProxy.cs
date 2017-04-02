/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;

using GroceryCo.Checkout.Domain;

namespace GroceryCo.Checkout.Client
{
    // To get Product's Name, Price and Promotions info
    public class ProductCatalogFileProxy : BaseFileProxy, ICatalogProvider
    {
        public ProductCatalogFileProxy() { }

        public ProductCatalogFileProxy(string catalogFile) 
        {
            this.Load(catalogFile);
        }

        // assign to this.products
        private void ParseProductNodes(XmlDocument xmlDoc)
        {
            var xmlNodes = xmlDoc.DocumentElement.SelectNodes("/Catalog/Items/Item");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                var product = new Product();

                if (xmlNode["Name"] != null)
                {
                    if (xmlNode["Name"].InnerText.Trim() != "")
                        product.Name = xmlNode["Name"].InnerText.Trim().ToUpper();
                    else                     // product name is mandatory
                        throw new ProductNameEmptyException();
                }
                else
                    throw new ProductNameNodeMissedException(string.Format("ProductCatalogFileProxy: Product {0} <Name> missed", product.Name));

                if (xmlNode["Price"] != null)
                {
                    decimal price;
                    if (decimal.TryParse(xmlNode["Price"].InnerText.Trim(), out price))
                        product.Price = price;
                    else
                        throw new InvalidProductPriceNodeException(string.Format("ProductCatalogFileProxy: Invalid <Price> value for Product {0}, decimal expected", product.Name));
                }
                else
                    throw new ProductPriceNodeMissedException(string.Format("ProductCatalogFileProxy: Product {0} <Price> missed", product.Name));

                if (!this.products.ContainsKey(product.Name))
                    this.products.Add(product.Name, product);
                else
                    throw new DuplicatedProductException(string.Format("ProductCatalogFileProxy.Parse(): duplicated catalog product addressed: {0}", product.Name));
            }
        }

        // assign to this.promotions
        private void ParsePromotionNodes(XmlDocument xmlDoc)
        {
            var xmlNodes = xmlDoc.DocumentElement.SelectNodes("/Catalog/Promotions/Promotion");
            foreach (XmlNode promotionNode in xmlNodes)      // xmlNode.SelectNodes("Promotions/Promotion"))
            {
                if (promotionNode.Attributes["Name"] != null)
                {
                    var promotionName = promotionNode.Attributes["Name"].Value.Trim();

                    // .Create will throw exception if no matching promotion implementation found
                    var promotion = BaseProductPromotion.Create(promotionName) as IPromotion;
                    this.promotions.Add(promotion);

                    if (promotionNode.Attributes["Rule"] != null)
                        promotion.Rule = promotionNode.Attributes["Rule"].Value.Trim(); // RuleModel will further validate..
                    else
                        throw new PromotionRuleAttributeMissedException(string.Format("{0} Rule attribute missed", promotion.GetType().Name));

                    // promotion item sub node
                    foreach (XmlNode promotionItemNode in promotionNode.SelectNodes("Items/Item"))
                    {
                        if (promotionItemNode.Attributes["Name"] != null)
                            (promotion as IProductPromotion).Products.Add(promotionItemNode.Attributes["Name"].Value.Trim());
                        else 
                            throw new ProductNameNodeMissedException(string.Format("Promotion {0}'s <Item> Name attribute missed", promotion.GetType().Name));
                    }

                }
                else
                    throw new PromotionNameAttributeMissedException();
            }
        }

        public override void Parse(XmlDocument xmlDoc) 
        {
            this.ParseProductNodes(xmlDoc);
            this.ParsePromotionNodes(xmlDoc); 
        }

        #region Implement IProductProvider

        private List<IPromotion> promotions = new List<IPromotion>();
        public List<IPromotion> Promotions
        {
            get { return this.promotions; }
        }

        // key will be Product.Name but .Trim()'ed and .ToUpper()'ed
        private Dictionary<string, Product> products = new Dictionary<string, Product>();
        public  Dictionary<string, Product> Products
        {
            get { return this.products; }
        }

        // Get Product info by name
        public Product GetProduct(string name)
        {
            var key = name.Trim().ToUpper();

            if (this.products.ContainsKey(key))
                return this.products[key];
            else
            {
                string msg = string.Format("ProductCatalogFileProxy.GetProduct: product name '{0}' not found in catalog", key);
                //Console.WriteLine(msg);
                throw new ProductNotFoundException(key);
            }
        }

        public decimal GetProductPrice(string name)
        {
            var key = name.Trim().ToUpper();
            var product = this.GetProduct(key);

            if (product != null)
                return product.Price;
            else
                return Decimal.Zero;
        }

        #endregion
    }
}
