using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kraken.Core;

namespace Kraken.Core
{
    public static class Enumeration
    {

        public static T FromValue<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T FromNumber<T>(byte value)
        {
            return FromNumber<T>((int) value);
        }

        public static T FromNumber<T>(int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }


        /// <summary>
        /// Return a list of all the enum values
        /// </summary>
        /// <remarks>
        /// Optionally exclude any enums that equate to zero - which typically signify illegal values
        /// </remarks>
        /// <param name="includeZeroValue">Whether or not to include enums that map to zero in the list</param>
        public static List<T> GetAll<T>(bool includeZeroValue = false)
        {
            Array enumArray = Enum.GetValues(typeof(T));
            List<EnumSortInformation<T>> sortableEnumList = new List<EnumSortInformation<T>>();
            foreach (T enumerationValue in enumArray)
            {
                if (Convert.ToInt32(enumerationValue) != 0 || includeZeroValue)
                {
                    sortableEnumList.Add(new EnumSortInformation<T>(enumerationValue));
                }
            }

            return sortableEnumList
                .OrderBy(x => x.SortOrder)
                .Select(x => x.Value)
                .ToList();
        }

        #region Enum Helper Class: EnumSortInformation
        /// <summary>
        /// Helper class to calculate and store the sort order for an enum value - uses a combination of <seealso cref="EnumSortOrderAttribute"/> and the integer value
        /// </summary>
        /// <typeparam name="TEnumType"></typeparam>
        private class EnumSortInformation<TEnumType>
        {
            #region Constructor

            public EnumSortInformation(TEnumType value)
            {
                Value = value;
                SortOrder = CalculateSortOrder();
            }

            #endregion

            #region Properties

            public TEnumType Value { get; private set; }

            public int SortOrder { get; private set; }

            #endregion

            #region Private Members

            private int CalculateSortOrder()
            {
                FieldInfo info = Value.GetType().GetField(Value.ToString());
                object[] attributes = info.GetCustomAttributes(typeof(EnumSortOrderAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((EnumSortOrderAttribute)attributes[0]).SortOrder;
                }
                else
                {
                    return Convert.ToInt32(Value);
                }
            }

            #endregion

        }
        #endregion
    }
}