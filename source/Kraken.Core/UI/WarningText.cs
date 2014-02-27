using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Core
{
    #region WarningLevel enum
    /// <summary>
    /// Pages can call RenderInfoText to send top level messages.
    /// This enum allows various levels of warnings and importance
    /// </summary>
    public enum WarningLevel
    {
        /// <summary>
        /// A helpful hint to a user
        /// </summary>
        Information = 1,

        /// <summary>
        /// A bit of a stronger warning
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Something that will prevent the user from progressing;
        /// </summary>
        Critical = 3
    }
    #endregion

    /// <summary>
    /// Encapsulates warnings and information passed up to the UI layer (at a page level)
    /// </summary>
    [Serializable]
    public class WarningText : IEquatable<WarningText>
    {
        #region Properties
        public WarningLevel Level { get; set; }

        public string Text { get; set; }

        /// <summary>
        /// Sorting hint when levels are the same. 1 = sort highest
        /// </summary>
        public int SortOrder { get;set; }
        #endregion

        #region Constructors
        public WarningText()
        {
            
        }

        public WarningText(WarningLevel level, string textFormat, params object[] args)
        {
            Level = level;
            Text = string.Format(textFormat, args);
        }
        #endregion

        #region Instance Methods
        public override string ToString()
        {
            return string.Format("[{0}] {1}", Level, Text);
        }

        public bool Equals(WarningText other)
        {
            return other != null && string.Compare(other.Text, Text, true) == 0 && other.Level == Level;
        }

        public override int GetHashCode()
        {
            return (Text + Level).GetHashCode();
        }
        #endregion
    }
}