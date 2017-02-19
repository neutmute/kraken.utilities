using System;
using System.Collections.Generic;

using System.Text;
using Kraken.Core.Instrumentation;

namespace Kraken.Core.Instrumentation
{
    public class ReportCollection : List<ReportBase>
    {
        #region Instance Methods
        public void Add(int sortOrder, string heading, KeyValuePair<string, string> nameValues)
        {
            Report report = new Report();
            report.Heading = heading;
            report.Data.Add(nameValues);
            report.SortOrder = sortOrder;

            Add(report);
        }

        public string Render(ReportFormat reportFormat)
        {
            StringBuilder stringBuilder = new StringBuilder();

            // Sort based on the order supplied in the reports
            this.Sort((x, y) => x.SortOrder.CompareTo(y.SortOrder));

            string separator = reportFormat == ReportFormat.Html ? "<BR />\r\n" : "\r\n\r\n";
            foreach (ReportBase report in this)
            {
                stringBuilder.Append(report.ToString(reportFormat));
                stringBuilder.Append(separator);
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
