using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kraken.Core.Extensions;
using NLog;

namespace Kraken.NLog
{
    public static class NLogConfig
    {
        private static string _logTargetFolder;

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Slight hack - assumes the first log folder in nlog is the log directory
        /// </summary>
        public static string LogFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_logTargetFolder))
                {
                    var logFileTargets = Log.GetFileTargets();

                    _logTargetFolder = Path.GetTempPath();
                    if (logFileTargets.Count > 0)
                    {
                        _logTargetFolder = Path.GetDirectoryName(logFileTargets[0]);
                    }
                    Directory.CreateDirectory(_logTargetFolder);
                }
                return _logTargetFolder;
            }
        }
    }
}
