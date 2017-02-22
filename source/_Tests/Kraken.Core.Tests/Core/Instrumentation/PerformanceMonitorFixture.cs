using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Instrumentation;
using Kraken.Tests.Extensions;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core.Instrumentation
{
    public class PerformanceMonitorFixture : Fixture
    {
        [Test]
        public void EmitSummary_Format_Normal()
        {
            PerformanceMonitor perfMonitor = new PerformanceMonitor();
            perfMonitor.Description = "Perf Mon is the SUT";

            DateTime baseTime = DateTime.Parse("2012-02-17 13:14");

            for (int i = 0; i < 10; i++)
            {
                var startTime = baseTime.AddSeconds(i);
                var duration = new TimeSpan(0,0,0,i,i*20);
                PerformancePoint point = new PerformancePoint("PointName", baseTime.AddSeconds(i), duration);
                perfMonitor.LogPoint(point);
            }

            string summary = perfMonitor.GetSummary();
            Console.WriteLine(summary);

            // AssertBuilder.Generate(summary, "summary"); // The following assertions were generated on 17-Feb-2012
            #region Generated Assertions
            Assert.AreEqual(@"
--------------------------------------------------------------------------------
Performance Monitor Output: Perf Mon is the SUT
--------------------------------------------------------------------------------
  Name       Hits   Total (ms)  Average (ms)  Maximum (ms)                        Minimum (ms)                  
  ---------  -----  ----------  ------------  ----------------------------------  ------------------------------
  PointName     10   45,900.00      4,590.00  Duration=9,180.00ms, Time=13:14:09  Duration=0.00ms, Time=13:14:00
--------------------------------------------------------------------------------
".NormaliseCrlf(), summary.NormaliseCrlf());
            #endregion
        }
    }
}