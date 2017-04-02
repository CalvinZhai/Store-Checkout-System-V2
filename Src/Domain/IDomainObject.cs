/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    interface IDomainObject
    {
        void Log(string info);                 // log whatever exception or info for future bug investigation 
    }
}
