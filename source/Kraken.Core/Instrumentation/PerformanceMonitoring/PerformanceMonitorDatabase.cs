using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Instrumentation;

namespace Kraken.Core.Instrumentation
{
    public static class PerformanceMonitorDatabase
    {
        private static readonly PerformanceMonitor instance = new PerformanceMonitor();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static PerformanceMonitorDatabase()
        {
        }

        public static PerformanceMonitor Instance
        {
            get
            {
                return instance;
            }
        }
    } 
}
