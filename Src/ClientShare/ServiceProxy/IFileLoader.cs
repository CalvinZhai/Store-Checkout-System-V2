using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Client
{
    // Load local XML file
    public interface IFileLoader
    {
        void Load(string filePath);         // 
    }
}
