using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Client
{
    public interface IProxy
    {
        void Init();                        // Initialize, e.g. to connect to backend service
    }
}
