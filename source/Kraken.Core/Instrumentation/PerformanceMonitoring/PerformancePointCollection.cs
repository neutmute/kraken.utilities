using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    public class PerformancePointCollection : List<PerformancePoint>
    {
        public PerformanceSummary GetSummary()
        {
            PerformanceSummary summary = new PerformanceSummary();

            summary.TotalTime = new TimeSpan();
            ForEach(d => summary.TotalTime += d.TimeSpan);

            summary.Hits = Count;
            summary.Minimum = Find(d => d.TimeSpan == this.Min(dp => dp.TimeSpan));
            summary.Maximum = Find(d => d.TimeSpan == this.Max(dp => dp.TimeSpan));

            return summary;
        }
    }
}
