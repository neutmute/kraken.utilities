using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    public class ReportHtml : ReportText
    {
        public override bool IsHtmlFormatted
        {
            get
            {
                 return true;
            }
        }
    }
}
