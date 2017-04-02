using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    // Product level promotion
    public interface IProductPromotion : IPromotion
    {
        List<string> Products { get; set; }       // applicable product names, .Trim().ToUpper(), "*" means all products
    }
}
