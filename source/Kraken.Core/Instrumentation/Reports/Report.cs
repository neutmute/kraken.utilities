using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    public enum ReportFormat
    {
        Unspecified = 0,
        Plain = 1,
        Html = 2
    }

    public class Report : ReportBase
    {
        #region Properties
        public NameValueCollection Data { get; private set; }

        public override bool HasContent
        {
            get { return Data.Count > 0; }
        }
        #endregion

        public Report()
        {
            // Content = new StringBuilder();
            Data = new NameValueCollection();
        }
    }
}

