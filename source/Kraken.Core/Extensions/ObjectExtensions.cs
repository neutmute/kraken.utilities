using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Extensions
{
    internal static class ObjectExtensions
    {
        public static string ToStringNullSafe(this object target)
        {
            if (target == null)
            {
                return "<NULL>";
            }
            return target.ToString();
        }
    }
}
