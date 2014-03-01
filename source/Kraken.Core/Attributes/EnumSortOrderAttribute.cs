using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core
{
    /// <summary>
    /// This attribute can be applied to an Enum field to control how the Enum is sorted when being displayed to the user
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumSortOrderAttribute : Attribute
    {
        #region Constructors

        public EnumSortOrderAttribute(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        #endregion

        #region Properties

        public int SortOrder { get; private set; }

        #endregion
    }
}
