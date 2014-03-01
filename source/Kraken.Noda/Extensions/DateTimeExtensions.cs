using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodaTime;

namespace Kraken.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static Instant ToInstant(this DateTimeOffset dateTimeOffset)
        {
            return Instant.FromDateTimeOffset(dateTimeOffset);
        }
    }
}
