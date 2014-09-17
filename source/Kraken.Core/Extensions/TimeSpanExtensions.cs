using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    [Flags]
    public enum HumanReadableTimeSpanOptions
    {
        /// <summary>
        /// Hours Minutes Seconds
        /// </summary>
        Verbose,

        /// <summary>
        /// Hrs Mins Secs
        /// </summary>
        Abbreviated,
    }

    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts a timespan to a human readable format,
        /// eg. d days, h hours, m mins, s secs.
        /// </summary>
        public static string ToHumanReadable(this TimeSpan timeSpan, int maxResolutionDepth = 99)
        {
            return ToHumanReadable(timeSpan, HumanReadableTimeSpanOptions.Verbose, maxResolutionDepth);
        }

        /// <summary>
        /// Converts a timespan to a human readable format,
        /// eg. d days, h hours, m mins, s secs.
        /// 
        /// Based off some potty mouth amateur web monkey
        /// http://www.codekeep.net/snippets/dc060497-9e0c-4a60-b1ed-aff6127fb80b.aspx
        /// </summary>
        /// <returns>Human readable time duration.</returns>
        public static string ToHumanReadable(this TimeSpan timeSpan, HumanReadableTimeSpanOptions options, int maxResolutionDepth = 99)
        {
            decimal seconds = Convert.ToDecimal(timeSpan.TotalSeconds);
            string dayLabel = "day";
            string hourLabel = "hour";
            string minuteLabel = "minute";
            string secondLabel = "second";
            string millisecondLabel = "milliseconds";

            var timeSinceEpoch = SystemDate.Now - DateTime.MinValue;
            if (timeSinceEpoch.Days == timeSpan.Days)
            {
                return "never";
            }

            if (options == HumanReadableTimeSpanOptions.Abbreviated)
            {
                dayLabel = "day";
                hourLabel = "hr";
                minuteLabel = "min";
                secondLabel = "sec";
                millisecondLabel = "ms";
            }

            var resolutionDepth = 0;

            if (seconds == 0)
            {
                return string.Format("0 {0}s", secondLabel);
            }

            StringBuilder sb = new StringBuilder();
            if (seconds >= 86400)
            {
                sb.AppendFormat("{0} {2}{1}"
                    , (long)seconds / 86400
                    , seconds >= 86400 * 2 ? "s" : string.Empty
                    , dayLabel);

                seconds -= (long)(seconds / 86400) * 86400;
                resolutionDepth++;
            }
            if (seconds >= 3600 && resolutionDepth < maxResolutionDepth)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(
                    "{0} {1}{2}"
                    , (int)seconds / 3600
                    , hourLabel
                    , seconds >= 3600 * 2 ? "s" : string.Empty
                    );
                seconds -= (int)(seconds / 3600) * 3600;
                resolutionDepth++;
            }
            if (seconds >= 60 && resolutionDepth< maxResolutionDepth)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(
                    "{0} {1}{2}"
                    , (int)seconds / 60
                     , minuteLabel
                    , seconds >= 60 * 2 ? "s" : string.Empty
                    );
                seconds -= (int)(seconds / 60) * 60;
                resolutionDepth++;
            }
            if (seconds > 0 && resolutionDepth < maxResolutionDepth)
            {
                if (sb.Length > 0)
                {
                    sb.AppendFormat(
                        ", {0} {1}{2}"
                        , (int)seconds
                        , secondLabel
                        , seconds == 1 ? string.Empty : "s");
                    resolutionDepth++;
                }
                else
                {
                    if (seconds == (int)seconds)
                    {
                        sb.AppendFormat("{0} {1}s", (int)seconds, secondLabel);
                    }
                    else if (seconds > Decimal.One)
                    {
                        sb.AppendFormat("{0} {1}s", seconds.ToString("N2"), secondLabel);
                    }
                    else
                    {
                        sb.AppendFormat("{0} {1}", timeSpan.TotalMilliseconds, millisecondLabel);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
