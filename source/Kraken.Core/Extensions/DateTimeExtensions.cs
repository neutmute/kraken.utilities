using System;
using System.Collections.Generic;
using System.Globalization;
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
                return dateTime.Value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) + " " + dateTime.Value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);
            }
            return valueWhenNull;
        }

    }
}
