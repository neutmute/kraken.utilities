using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Common.Logging;

namespace Kraken.Core.Instrumentation
{
    public enum PerformanceMonitorMode
    {
        
        /// <summary>
        /// There will be many nested calls so LogBegin and LogEnd must be explicitly called
        /// </summary>
        Nested = 0,

        /// <summary>
        /// Allow implicit LogEnd
        /// </summary>
        Sequential,
    }

    /// <summary>
    /// Use to log and collect performance data on iterative/nested tasks
    /// </summary>
    public class PerformanceMonitor
    {
        #region Fields
        private readonly List<PerformancePoint> _dataPoints;
        private readonly Dictionary<string, PerformancePoint> _currentLogs;
        private string _mostRecentLogName;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties
        public string Description { get; set; }

        /// <summary>
        /// Allow the an inferral of the log end
        /// </summary>
        public PerformanceMonitorMode Mode { get; set; }

        private bool IsModeSequential
        {
            get { return Mode == PerformanceMonitorMode.Sequential; }
        }


        /// <summary>
        /// Set to true when loop and exec in Swordfish. False in IIS
        /// </summary>
        public bool ResetDataPointsOnRetrieval {get;set;}
        #endregion

        #region Constructors
        public PerformanceMonitor()
        {
            Mode = PerformanceMonitorMode.Nested;
            _currentLogs = new Dictionary<string, PerformancePoint>();
            _dataPoints = new List<PerformancePoint>();
        }

        public PerformanceMonitor(string descriptionFormat, params object[] args) : this()
        {
            Description = string.Format(descriptionFormat, args);
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Allow a less verbose logging syntax if there will be no log loops
        /// </summary>
        private void SequentialModeCleanup()
        {
            if (!string.IsNullOrEmpty(_mostRecentLogName) && IsModeSequential)
            {
                LogEnd(_mostRecentLogName);
            }
        }

        public void LogBegin(string name)
        {
            PerformancePoint point = new PerformancePoint();
            point.Name = name;
            point.StartTimer();

            // Violate bracing rules for coverage
            if (_currentLogs.ContainsKey(name))
                throw KrakenException.Create("Attempt to LogBegin('{0}') when there is already one in progress. Use RequireExplicitLogEnd=false if you are lazy.", name);

            
            SequentialModeCleanup();

            _currentLogs.Add(name, point);
            _mostRecentLogName = name;
        }

        public void LogEnd(string name)
        {
            if (!_currentLogs.ContainsKey(name))
                throw KrakenException.Create("Attempt to LogEnd('{0}') when one has not been started", name);
            
            PerformancePoint point = _currentLogs[name];

            point.StopTimer();

            // clear the tracker
            _currentLogs.Remove(name);
            LogPoint(point);
            _mostRecentLogName = null;
        }

        public void LogPoint(PerformancePoint point)
        {
            lock (_dataPoints)
            {
                _dataPoints.Add(point);
            }
        }

        /// <summary>
        /// Instruct the class to do the log.
        /// </summary>
        /// <remarks>
        /// This is useful for directing all perf logs to a separate logger
        /// eg:
        /// <logger name="Kraken.Core.Performance.PerformanceMonitor" minlevel="Info" writeTo="PerfMonitorFile" />
        /// </remarks>
        public void EmitSummary()
        {
            Log.Info(GetSummary());
        }

        /// <summary>
        /// Allow the consumer to do the logging
        /// </summary>
        public string GetSummary()
        {
            SequentialModeCleanup();
            StringBuilder summaryString = new StringBuilder();

            summaryString.AppendLine("\r\n".PadRight(82, '-'));
            summaryString.AppendFormat("Performance Monitor Output: {0}\r\n", Description);
            summaryString.AppendLine(string.Empty.PadRight(80, '-'));
            summaryString.Append(GetSummaryPlain());
            summaryString.AppendLine(string.Empty.PadRight(80, '-'));

            return summaryString.ToString();
        }

        private string GetSummaryPlain()
        {
            // Collate the data
            Dictionary<string, PerformancePointCollection> grouped = new Dictionary<string, PerformancePointCollection>();

            lock (_dataPoints)
            {
                foreach (PerformancePoint datum in _dataPoints)
                {
                    if (!grouped.ContainsKey(datum.Name))
                    {
                        grouped.Add(datum.Name, new PerformancePointCollection());
                    }
                    grouped[datum.Name].Add(datum);
                }
            }

            List<PerformanceSummary> perfSummarys = new List<PerformanceSummary>();

            foreach(string key in grouped.Keys)
            {
                PerformanceSummary summary = grouped[key].GetSummary();
                summary.Name = key;
                perfSummarys.Add(summary);
            }

            perfSummarys.Sort((p1, p2) => p1.TotalTime.CompareTo(p2.TotalTime));

            ObjectDumper<PerformanceSummary> tableDump = new ObjectDumper<PerformanceSummary>(PerformanceSummary.GetObjectDump);

            var summaryString = tableDump.Dump(perfSummarys);

            if (ResetDataPointsOnRetrieval)
            {
                _dataPoints.Clear();
            }

            return summaryString;
        }
        #endregion
    }
}
