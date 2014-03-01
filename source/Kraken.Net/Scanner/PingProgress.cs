using System;

namespace Kraken.Net
{
    public class PingProgress : ProgressEventArgs
    {
        public override int PercentComplete
        {
            get
            {
                return GetPercent(ScansCompleted, TotalToScan);
            }
        }

        public int ScansCompleted { get; set; }

        public int ReplyCount { get; set; }

        public int TotalToScan { get; set; }

        public override string ToString()
        {
            return String.Format("Complete={0}, Total={1}, Replies={2}, Percent={3}", ScansCompleted, TotalToScan, ReplyCount, PercentComplete);
        }
    }
}