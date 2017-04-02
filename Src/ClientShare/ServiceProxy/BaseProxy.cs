using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Client
{
    public abstract class BaseProxy : IProxy
    {
        // In a real SOA app, might do some service initialization work..., now we have nothing real to put into
        public virtual void Init() { }

    }
}
