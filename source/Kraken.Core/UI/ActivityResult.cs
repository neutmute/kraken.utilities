using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Core
{
    /// <summary>
    /// Contain the results of a test on an entity
    /// </summary>
    /// <remarks>
    /// Indicates test success/fail and contains a list of messages
    /// </remarks>
    public class ActivityResult
    {
        #region Properties
        public bool Success { get; set; }

        public WarningTextCollection Messages { get; set; }

        /// <summary>
        /// Indicate whether the messages have HTML within
        /// </summary>
        public bool IsHtmlFormatted { get; set; }
        #endregion

        #region Constructors
        public ActivityResult()
        {
            Success = true;
            Messages = new WarningTextCollection();
        }

        public static ActivityResult GetSuccess(string messageFormat, params object[] args)
        {
            var result = new ActivityResult();
            result.Messages.Add(WarningLevel.Information, messageFormat, args);
            return result;
        }


        public static ActivityResult GetError(string messageFormat, params object[] args)
        {
            var result = new ActivityResult();
            if (!string.IsNullOrEmpty(messageFormat))
            {
                result.Messages.Add(WarningLevel.Critical, messageFormat, args);
            }
            result.Success = false;
            return result;
        }
        #endregion

        #region Instance Methods
        public void Merge(ActivityResult result)
        {
            Success &= result.Success;
            Messages.AddRange(result.Messages);
        }

        public override string ToString()
        {
            return string.Format("Success={0}, Messages={1}", Success, Messages.ToCsv("|"));
        }


        public string ToString(string messageDelimiter)
        {
            return $"Success={Success}, Messages={messageDelimiter}{Messages.ToString(messageDelimiter)}";
        }
        #endregion
    }
}
