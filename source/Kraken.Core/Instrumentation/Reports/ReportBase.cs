using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    public abstract class ReportBase
    {
        #region Properties
        public int SortOrder { get; set; }

        public string Heading { get; set; }

        public abstract bool HasContent { get; }
        #endregion
        
        #region Instance Methods
        public string ToString(ReportFormat bodyFormat)
        {
            return ToString(bodyFormat, 0);
        }

        public string ToString(ReportFormat bodyFormat, int newlinePrepend)
        {
            ReportRenderer renderer = new ReportRenderer(bodyFormat);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < newlinePrepend; i++)
            {
                stringBuilder.Append(renderer.NewLine);
            }
            stringBuilder.Append(renderer.Render(this));
            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return ToString(ReportFormat.Plain);
        }
        #endregion
    }
}
