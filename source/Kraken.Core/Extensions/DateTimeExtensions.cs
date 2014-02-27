using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateTimeString(this DateTime? dateTime, string valueWhenNull = "(never)")
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToShortDateString() + " " + dateTime.Value.ToShortTimeString();
            }
            return valueWhenNull;
        }

    }
}
