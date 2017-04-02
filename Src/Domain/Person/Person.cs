using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public abstract class Person: BaseDomainObject, IPerson
    {
        // Customer Name
        public string Name { get; set; }

        // Format is not a big deal
        public string HomePhone { get; set; }       // e.g.1-403-456-7890

        // Format is not a big deal
        public string CellPhone { get; set; }       // e.g.4034567890

        // Sometime may be required for a customer
        public string Email { get; set; }

        // Sometimes store may require a customer's address info for either shipping or tracking purpose 
        // Let's just use a monkey string for now to represent an address
        public string Address { get; set; }
    }
}
