using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public class Customer : Person
    {
        // Function method to create a IProductPromotion instance based on passed-in 'promotionName'
        public static Customer Create(string className)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.Name == className && typeof(Customer).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).First();
            if (type != null)
                return (Customer)Activator.CreateInstance(type);
            else
                throw new CustomerNotImplementedException(string.Format("Customer.Create: {0} class not implemented", className));
        }

        public Customer() { }
    }
}
