using System;

namespace Kraken.Net
{
    public class ProgressEventArgs : EventArgs
    {
        public string CurrentTask { get; set; }
               
        public virtual int PercentComplete { get; set; }

        public ProgressEventArgs()
        {
            
        }

        public ProgressEventArgs(int progress, int total, string taskFormat, params object[] taskArgs)
        {
            PercentComplete = GetPercent(progress, total);
            CurrentTask = string.Format(taskFormat, taskArgs);
        }

        public ProgressEventArgs(int percentComplete, string taskFormat, params object[] taskArgs)
        {
            PercentComplete = percentComplete;
            CurrentTask = string.Format(taskFormat, taskArgs);
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}% complete", CurrentTask, PercentComplete);
        }

        protected int GetPercent(int progress, int total)
        {
            if (total == 0)
            {
                return 0;
            }
            return (progress * 100) / total;
        }
    }
}