using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Kraken.Core;
using Kraken.Core.Extensions;

namespace Kraken.Core
{
    /// <summary>
    /// Extension methods relating to the <see cref="System.Enum"/> class.
    /// </summary>
    /// <remarks>
    /// Reflection is used in these extension methods. be aware of performance considerations. 
    /// </remarks>
    public static class EnumExtensionMethods
    {
        /// <summary>
        /// Returns an instance of <see cref="System.Enum"/> type <typeparam name="T" /> which represents
        /// the first member decorated with an <see cref="EnumCodeAttribute"/> having the specified 
        /// <paramref name="code"/>. If no enum member has the specified code, the default value of the enum
        /// is returned.
        /// </summary>
        public static T ToEnumFromCode<T>(this string code)
        {
            FieldInfo[] fields = typeof(T).GetFields();
            foreach (FieldInfo t in fields)
            {
                EnumCodeAttribute[] attributes = (EnumCodeAttribute[])t.GetCustomAttributes(typeof(EnumCodeAttribute), false);
                if ((attributes.Length > 0) && (code == attributes[0].Code))
                {
                    return (T)Enum.Parse(typeof(T), t.Name);
                }
            }
            return (T)Enum.ToObject(typeof(T), 0);
        }


        /// <summary>
        /// If the specified <paramref name="enumValue"/> is decorated with an <see cref="EnumCodeAttribute"/>, this
        /// method returns the value <see cref="EnumCodeAttribute.Code"/> property of that attribute, otherwise it 
        /// returns the numeric representation of the enum value.
        /// </summary>
        public static string GetCode(this Enum enumValue)
        {
            EnumCodeAttribute attribute = GetAttribute<EnumCodeAttribute>(enumValue);
            if (attribute != null)
            {
                return attribute.Code;
            }
            return Convert.ToInt32(enumValue).ToString();
        }

        /// <summary>
        /// If the specified <paramref name="enumValue"/> is decorated with an <see cref="DescriptionAttribute"/>, this
        /// method returns the value <see cref="DescriptionAttribute.Description"/> property of that attribute, otherwise it 
        /// returns the result of the enum value's <see cref="Enum.ToString()"/> method.
        /// </summary>
        public static string GetDescription(this Enum enumValue)
        {
            DescriptionAttribute attribute = GetAttribute<DescriptionAttribute>(enumValue);
            if (attribute != null)
            {
                return attribute.Description;
            }
            return enumValue.ToString();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            var attribute = GetAttribute<EnumDisplayNameAttribute>(enumValue);
            if (attribute != null)
            {
                return attribute.Name;
            }
            return enumValue.ToString().SplitCamelCase().ToCsv(' ', s => s);
        }

        /// <summary>
        /// Gets the first <see cref="System.Attribute"/> of type T that the 
        /// <paramref name="enumValue"/> is decorated with, or the default of T
        /// if the enum value is not decorated with any attributes of that type.
        /// </summary>
        /// <typeparam name="T">A <see cref="Type"/> that inherits from <see cref="Attribute"/>.</typeparam>
        public static T GetAttribute<T>(this Enum enumValue)
        {
            object[] attributes = enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(T), false);
            if (attributes.Length > 0)
            {
                return (T)attributes[0];
            }
            return default(T);
        }

        public static string ToCsvDisplay(this IEnumerable<Enum> enumValues, string joiner = ", ")
        {
            return enumValues.ToCsv(joiner, e => e.GetDisplayName());
        }

    }
}
