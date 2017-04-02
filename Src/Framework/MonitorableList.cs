/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Checkout.Framework
{
    /// <summary>
    /// So we can monitor something e.g. List.Add to do something fancy like preventing from adding duplicated items
    /// Not used so far...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonitorableList<T> : List<T>
    {
        public event EventHandler OnAdd;

        public void Add(T item)
        {
            if (OnAdd != null)
            {
                this.OnAdd(this, null);
            }

            base.Add(item);
        }
    }
}
