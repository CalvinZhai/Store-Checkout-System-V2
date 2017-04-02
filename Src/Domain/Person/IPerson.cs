using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    public interface IPerson
    {
        string Name { get; set; }
        string HomePhone { get; set; }
        string CellPhone { get; set; }
        string Email { get; set; }
        string Address { get; set; }
    }
}
