using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class PromotionRuleEmptyException : BaseException
    {
        public PromotionRuleEmptyException() { }
        public PromotionRuleEmptyException(string message) : base(message) { }
    }

    // Assign a promotion in product catalog which has NOT been implemented yet
    public class PromotionNotImplementedException : BaseException
    {
        public PromotionNotImplementedException() { }
        public PromotionNotImplementedException(string message) : base(message) { }
    }

    // Not empty but in invalid rule format, e.g. for "buy 3 get 2 free", expected "3-2" but with actual "3" or "3-abc"
    public class InvalidPromotionRuleException : BaseException
    {
        public InvalidPromotionRuleException() { }
        public InvalidPromotionRuleException(string message) : base(message) { }
    }
}
