using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GroceryCo.Checkout.Domain;

namespace GroceryCo.Checkout.Client
{
    // likely in product catalog, <Name> missed
    public class ProductNameNodeMissedException : BaseException
    {
        public ProductNameNodeMissedException() { }
        public ProductNameNodeMissedException(string message) : base(message) { }
    }

    // likely in product catalog, <Price> missed
    public class ProductPriceNodeMissedException : BaseException
    {
        public ProductPriceNodeMissedException() { }
        public ProductPriceNodeMissedException(string message) : base(message) { }
    }

    // likely in product catalog, <Price> .Trim() value cannot convert to decimal, e.g. either ="" or "abc"
    public class InvalidProductPriceNodeException : BaseException
    {
        public InvalidProductPriceNodeException() { }
        public InvalidProductPriceNodeException(string message) : base(message) { }
    }

    // likely in product catalog
    public class DuplicatedProductException : BaseException
    {
        public DuplicatedProductException() { }
        public DuplicatedProductException(string message) : base(message) { }
    }

    // likely not found in product catalog when searching
    public class ProductNotFoundException : BaseException
    {
        public ProductNotFoundException() { }
        public ProductNotFoundException(string message) : base(message) { }
    }

    // likely in product catalog 
    public class PromotionNameAttributeMissedException : BaseException
    {
        public PromotionNameAttributeMissedException() { }
        public PromotionNameAttributeMissedException(string message) : base(message) { }
    }

    // likely in product catalog 
    public class PromotionRuleAttributeMissedException : BaseException
    {
        public PromotionRuleAttributeMissedException() { }
        public PromotionRuleAttributeMissedException(string message) : base(message) { }
    }
}
