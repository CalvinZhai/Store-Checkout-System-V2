/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class BaseException : System.Exception
    {
        public BaseException() { }
        public BaseException(string message) : base(message) { }
    }

    public class ProductNameEmptyException : BaseException
    {
        public ProductNameEmptyException() { }
        public ProductNameEmptyException(string message) : base(message) { }
    }

    public class ProductPriceLessThanZeroException : BaseException
    {
        public ProductPriceLessThanZeroException() { }
        public ProductPriceLessThanZeroException(string message) : base(message) { }
    }

    public class ProductQuantityLessThanZeroException : BaseException
    {
        public ProductQuantityLessThanZeroException() { }
        public ProductQuantityLessThanZeroException(string message) : base(message) { }
    }
}
