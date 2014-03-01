using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    public class PerformanceSummary
    {
        #region Properties
        public string Name { get; set; }

        public int Hits { get; set; }

        public TimeSpan TotalTime { get; set; }

        public PerformancePoint Maximum { get; set; }

        public PerformancePoint Minimum { get; set; }

        public double AverageMilliseconds
        {
            get
            {
                return TotalTime.TotalMilliseconds / Hits;
            }
        }
        #endregion

        #region Instance Methods
        public static ObjectDump GetObjectDump(PerformanceSummary target)
        {
            var dump = new ObjectDump();
            dump.Headers.AddRange(new []{"Name", "Hits", "Total (ms)", "Average (ms)", "Maximum (ms)", "Minimum (ms)"});
            dump.Data.AddRange(new[] { 
                target.Name, 
                target.Hits.ToString().PadLeft(5,' '), 
                target.TotalTime.TotalMilliseconds.ToString("N2").PadLeft(10,' '), 
                target.AverageMilliseconds.ToString("N2").PadLeft(12,' '), 
                target.Maximum.ToString(PerformancePointOutputOptions.WithStartTime)
                ,target.Minimum.ToString(PerformancePointOutputOptions.WithStartTime) });

            return dump;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            var dump = GetObjectDump(this);
            for (int i = 0; i < dump.Data.Count; i++)
            {
                stringBuilder.AppendFormat("{0}={1}, ", dump.Headers[i], dump.Data[i]);
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
