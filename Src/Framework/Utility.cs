/* 
 *              Copyright © 2017 Calvin Zhai.  All Rights Reserved.^_^^_^^_^
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Checkout.Framework
{
    /// <summary>
    /// Just provides with some general usage static functions for sharing
    /// NOT used so far
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Round the money
        /// e.g. C1Round(1.234, 2) returns 1.23
        /// e.g. C1Round(1.235, 2) returns 1.24
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static double C0Round(double value, int digit)
        {
            double vt = Math.Pow(10, digit);
            double vx = value * vt;

            vx += 0.50;
            return (Math.Floor(vx) / vt);
        }

        public static double C1Round(double value)
        {
            return (C0Round(value, 2));
        }


    }
}
