using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodaTime;

namespace Kraken.Core.Extensions
{
    public static class InstantExtensions
    {
        public static DateTimeOffset? ToDateTimeOffset(this Instant? instant)
        {
            if (instant == null)
            {
                return null;
            }
            return instant.Value.ToDateTimeOffset();
        }

        public static string ToShortDateString(this Instant instant)
        {
            var offset = instant.ToDateTimeOffset();
            return offset.LocalDateTime.ToShortDateString();
        }

        public static string ToShortTimeString(this Instant instant)
        {
            var offset = instant.ToDateTimeOffset();
            return offset.LocalDateTime.ToShortTimeString();
        }

        public static string ToShortDateTimeString(this Instant instant)
        {
            return instant.ToShortDateString() + " " + instant.ToShortTimeString();
        }

        public static string ToShortDateTimeString(this Instant? instant)
        {
            if (instant == null)
            {
                return string.Empty;
            }

            return instant.Value.ToLocalDateTime().ToShortDateString();
        }

        public static string ToShortTimeString(this Instant? instant)
        {
            if (instant == null)
            {
                return string.Empty;
            }

            var offset = instant.Value.ToDateTimeOffset();
            return offset.LocalDateTime.ToShortTimeString();
        }

        public static DateTime ToLocalDateTime(this Instant instant)
        {
            var offset = instant.ToDateTimeOffset();
            return offset.LocalDateTime;
        }
    }
}
