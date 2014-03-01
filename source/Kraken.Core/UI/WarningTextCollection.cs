using System;
using System.Collections.Generic;
using System.Text;

namespace Kraken.Core
{
    /// <summary>
    /// Models should return this collection for warnings and not a list of string
    /// </summary>
    [Serializable]
    public class WarningTextCollection : List<WarningText>
    {
        #region Constructors
        public WarningTextCollection()
        {
                
        }

        public WarningTextCollection(IEnumerable<WarningText> seed)
        {
            AddRange(seed);
        }
        #endregion

        #region Instance Methods
        public void Add(WarningLevel level, string format, params object[] args)
        {
            Add(level, 0, format, args);
        }

        public void Add(WarningLevel level, int sortOrder, string format, params object[] args)
        {
            WarningText warning = new WarningText {Level = level, Text = string.Format(format, args), SortOrder = sortOrder};
            Add(warning);
        }

        public void Add(string format, params object[] args)
        {
            Add(WarningLevel.Information, format, args);
        }

        public new WarningTextCollection FindAll(Predicate<WarningText> match)
        {
            WarningTextCollection subset = new WarningTextCollection();
            subset.AddRange(base.FindAll(match));
            return subset;
        }

        /// <summary>
        /// Filter out a subset based on warning level
        /// </summary>
        public WarningTextCollection FindAll(WarningLevel level)
        {
            return FindAll(l => l.Level == level);
        }

        public string ToString(string delimiter)
        {
            return this.ToCsv(delimiter, w => string.Format("[{0}] {1}", w.Level.ToString()[0], w.Text));
        }

        public override string ToString()
        {
            return ToString("\r\n");
        }

        public new void Sort()
        {
            Sort((x,y) =>
                     {
                         int value = y.Level.CompareTo(x.Level);
                         if (value == 0)
                         {
                             value = x.SortOrder.CompareTo(y.SortOrder);
                         }
                         return value;
                     });
        }
        #endregion
    }
}