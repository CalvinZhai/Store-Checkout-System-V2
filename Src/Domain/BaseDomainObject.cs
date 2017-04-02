/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Domain
{
    // supposed this implementation is a traditional RDBMS based
    // just show the spirit a bit here...
    public abstract class BaseDomainObject : IDomainObject
    {
        #region Implement interface of IDomainObject

        // In practice, you may want to use Microsoft Enterprise Lib to actually do the logging
        public virtual void Log(string info)
        {
            Console.WriteLine(info);        // let's simply log to console for now
        }

        #endregion
    }
}
