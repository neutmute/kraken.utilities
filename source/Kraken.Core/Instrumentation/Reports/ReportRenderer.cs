using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    /// <summary>
    /// Renders a report to Html or Text format
    /// </summary>
    public class ReportRenderer
    {
        #region Fields
        private ReportFormat _reportFormat;
        #endregion

        public ReportRenderer(ReportFormat reportFormat)
        {
            _reportFormat = reportFormat;
        }

        #region Properties
        protected bool IsHtmlRender
        {
            get { return _reportFormat == ReportFormat.Html; }
        }

        private string TableFormatHeading
        {
            get { return IsHtmlRender ? "<h2>{0}</h2>\r\n" : "*{0}*\r\n"; }
        }

        private string TableFormatTable
        {
            get { return IsHtmlRender ? "<table class='alarmTable' cellspacing='0' border='0' style='border-collapse:collapse;'>{0}</table>\r\n" : "{0}"; }
        }

        private string TableFormatLine
        {
            get { return IsHtmlRender ? "<tr>{0}</tr>\r\n" : "{0}\r\n"; }
        }

        private string TableFormatNameValue
        {
            get { return IsHtmlRender ? "<td>{0}</td><td>{1}</td>" : "- {0}: {1}"; }
        }

        private string TableFormatComment
        {
            get { return IsHtmlRender ? "{0}<br/>\r\n" : "{0}\r\n"; }
        }

        public string NewLine
        {
            get { return IsHtmlRender ? "<br/>" : "\r\n"; }
        }
        #endregion
        
        #region Instance Methods
        public string Render(ReportBase report)
        {
            ReportText reportText = report as ReportText;
            Report reportPairs = report as Report;

            string content = string.Empty;

            if (reportText != null)
            {
                string format = "{0}\r\n";
                string data = reportText.Data;
                if (IsHtmlRender && !reportText.IsHtmlFormatted)
                {
                    format = "<pre>{0}</pre>\r\n";
                    data = data.Replace("<", "&lt;");   // when an exception is from a web service and it contains HTML, we need to escape it out
                }

                if (!string.IsNullOrEmpty(reportText.Heading))
                {
                    content += string.Format(TableFormatHeading, reportText.Heading);
                }

                content += string.Format(format, data);
            }

            if (reportPairs != null)
            {
                content = GetFormattedTable(reportPairs.Heading, reportPairs.Data);
            }

            return content;
        }


        private string GetFormattedTable(string heading, NameValueCollection valuePairs)
        {
            //if (!IsHtmlRender)
            //{
            //    return GetObjectDump(valuePairs);
            //}
            StringBuilder content = new StringBuilder();
            content.AppendFormat(TableFormatHeading, heading);

            if (valuePairs != null && valuePairs.Count > 0)
            {
                StringBuilder allLineData = new StringBuilder();
                foreach (string key in valuePairs.Keys)
                {
                    string nameValuePair = string.Format(TableFormatNameValue, key, valuePairs[key]);
                    allLineData.Append(string.Format(TableFormatLine, nameValuePair));
                }
                content.AppendFormat(TableFormatTable, allLineData);
            }
            else
            {
                content.AppendFormat(TableFormatComment, "(nil)");
            }
            return content.ToString();
        }

        //private static string GetObjectDump(NameValueCollection nameValuePairs)
        //{
        //    List<ObjectDump> dumpList = new List<ObjectDump>();
        //    foreach (string key in nameValuePairs.AllKeys)
        //    {
        //        var dump = new ObjectDump(); 
        //        dump.Headers = new List<string> { "Name", "Value" };
        //        dump.Data = new List<string> { key, nameValuePairs[key] };
        //        dumpList.Add(dump);
        //    }
        //    return new ObjectDumper<ObjectDump>().Dump(dumpList);
        //}
        #endregion
    }
}
