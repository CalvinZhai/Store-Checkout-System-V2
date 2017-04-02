using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    // Used to map IProductPromotion.Rule string to object model, being set by BasePromotion.ParseRule()
    // slightly different naming convention that Members more follow JavaScript naming pattern than C# standard 
    // and the reason behind is to make promotion rule factors as close to humanbeing's language as possible
    public abstract class BaseRuleModel
    {
        // public string camelNamedProperty
    }
}
