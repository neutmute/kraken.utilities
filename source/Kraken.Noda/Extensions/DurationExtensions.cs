using NodaTime;

namespace Kraken.Core
{
    public static class DurationExtensions
    {
        public static string ToHumanReadable(this Duration duration, int maxResolutionDepth = 99)
        {
            return duration.ToTimeSpan().ToHumanReadable(maxResolutionDepth);
        }

        public static string ToHumanReadable(this Duration duration, HumanReadableTimeSpanOptions options, int maxResolutionDepth = 99)
        {
            return duration.ToTimeSpan().ToHumanReadable(options, maxResolutionDepth);
        }
    }
}