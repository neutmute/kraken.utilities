using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Instrumentation
{
    public class ReportText : ReportBase
    {
        #region Properties
        public string Data { get; set; }

        /// <summary>
        /// Tells the rendererer about the content of this report
        /// </summary>
        public virtual bool IsHtmlFormatted {get {return false;}}

        public override bool HasContent
        {
            get { return !string.IsNullOrEmpty(Data); }
        }
        #endregion
    }
}
