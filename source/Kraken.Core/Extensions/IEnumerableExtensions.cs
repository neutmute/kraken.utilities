using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Extensions;

namespace Kraken.Core
{
    public static class IEnumerableExtensions
    {
        public static string ToCsv<T>(this IEnumerable<T> source)
        {
            return ToCsv(source, ",");
        }

        public static string ToCsv<T>(this IEnumerable<T> source, string joiner)
        {
            var csv = String.Join(joiner, source.Select(x => x.ToStringNullSafe()).ToArray());
            return csv;
        }

        public static string ToCsv<T>(this IEnumerable<T> source, string joiner, Converter<T, string> converterMethod)
        {
            string csv = String.Join(joiner, source.Select(x => converterMethod(x)).ToArray());
            return csv;
        }

        public static string ToCsv<T>(this IEnumerable<T> source, char joiner, Converter<T, string> converterMethod)
        {
            var csv = ToCsv(source, joiner.ToString(), converterMethod);
            return csv;
        }

        #region FromCsv Methods
        /// <summary>
        /// Convert a csv string into a list of a specific type
        /// </summary>
        /// <remarks>
        /// Linq would be nice to use here with the IEnumerable(T).ToList but still targeting 2.0
        /// </remarks>
        public static void AddRange<T>(this List<T> target, string csvInput, Converter<string, T> converterMethod, params string[] delimiters)
        {
            if (string.IsNullOrEmpty(csvInput))
            {
                return;
            }

            string[] splitArray = csvInput.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            List<string> splitList = new List<string>();
            splitList.AddRange(splitArray);

            List<T> newEntries = splitList.ConvertAll(converterMethod);
            target.AddRange(newEntries);
        }

        /// <summary>
        /// Convert a csv string into a list of a specific type
        /// </summary>
        public static void AddRange<T>(this List<T> target, string csvInput, Converter<string, T> converterMethod)
        {
            AddRange(target, csvInput, converterMethod, ",");
        }
        #endregion
    }
}
