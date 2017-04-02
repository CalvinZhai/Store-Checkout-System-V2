/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public interface ICatalogProvider
    {
        Dictionary<string, Product> Products { get; }       // return all products
        Product GetProduct(string name);                    // return a product by its name

        List<IPromotion> Promotions { get; }
    }
}
