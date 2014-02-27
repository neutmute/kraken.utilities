using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;

namespace Kraken.Core.Extensions
{
    public static class LoggerExtensions
    {
        public static List<string> GetFileTargets(this Logger log)
        {
            List<string> targets = new List<string>();
            foreach (Target target in LogManager.Configuration.AllTargets)
            {
                FileTarget fileTarget = target as FileTarget;

                if (fileTarget != null)
                {
                    LogEventInfo lei = new LogEventInfo { TimeStamp = SystemDate.Now };
                    targets.Add(fileTarget.FileName.Render(lei));
                }
            }
            return targets;
        }
    }
}
