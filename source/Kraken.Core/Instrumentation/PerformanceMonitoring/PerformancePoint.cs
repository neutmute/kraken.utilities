using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    [Flags]
    public enum PerformancePointOutputOptions
    {
        Unknown = 0,
        Bare = 1,
        WithName = 2,
        WithStartTime = 4,
        Default = WithName | WithStartTime
    }

    public class PerformancePoint
    {
        #region Properties
        public string Name { get; set; }
        private TimeSpan _elapsed;
        private Stopwatch Stopwatch { get; set; }
        private DateTime DateStart { get; set; }

        public TimeSpan TimeSpan
        {
            get { return _elapsed; }
        }
        #endregion

        #region Ctor

        public PerformancePoint()
        {
            
        }

        public PerformancePoint(string name, DateTime started, TimeSpan elapsed)
        {
            Name = name;
            DateStart = started;
            _elapsed = elapsed;
        }

        #endregion
        
        #region Instance Methods
        
        
        public void StartTimer()
        {
            Stopwatch = Stopwatch.StartNew();
            DateStart = DateTime.Now;
        }

        public TimeSpan StopTimer()
        {
            Stopwatch.Stop();
            _elapsed = Stopwatch.Elapsed;
            return _elapsed;
        }

        public string ToString(PerformancePointOutputOptions options)
        {
            var withName = (options & PerformancePointOutputOptions.WithName) > 0;
            var withStartTime = (options & PerformancePointOutputOptions.WithStartTime) > 0;
            var bare = (options & PerformancePointOutputOptions.Bare) > 0;

            var bareDescription = TimeSpan.TotalMilliseconds.ToString("N2") + "ms";

            if (bare)
            {
                return bareDescription;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Duration={0}", bareDescription);

            if (withStartTime)
            {
                sb.AppendFormat(", Time={0:HH:mm:ss}", DateStart);    
            }
            
            if (withName)
            {
                sb.Insert(0, "Name=" + Name + ", ");
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(PerformancePointOutputOptions.Default);
        }
        #endregion

        #region Static Methods
        public static PerformancePoint Start(string name)
        {
            var perfPoint = new PerformancePoint { Name = name };
            perfPoint.StartTimer();
            return perfPoint;
        }

        public static PerformancePoint Start()
        {
            return Start(string.Empty);
        }
        #endregion

    }
}
