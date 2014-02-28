using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Targets;
using NLog;

namespace Kraken.Tests
{
    /// <summary>
    /// Oh noes. brittle testing logic!
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public static MemoryTarget GetMemoryTarget(this Logger log)
        {
            return GetMemoryTarget(log, "${message}", LogLevel.Info);
        }

        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public static MemoryTarget GetMemoryTarget(this Logger log, LogLevel logLevel)
        {
            return GetMemoryTarget(log, "${message}", logLevel);
        }

        /// <summary>
        /// Get a target to allow assertions to be made against the Nlog
        /// </summary>
        public static MemoryTarget GetMemoryTarget(this Logger log, string layout, LogLevel logLevel)
        {
            MemoryTarget memoryTarget = new MemoryTarget { Layout = layout };
            NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(memoryTarget, logLevel);
            return memoryTarget;
        }
    }
}
