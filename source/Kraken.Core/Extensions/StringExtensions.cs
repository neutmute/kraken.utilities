using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Core.Extensions
{
    public static class StringExtensions
    {
        public static string StripControlCharacters(this string value)
        {
            if (value == null) return null;

            StringBuilder newString = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                char ch = value[i];

                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
            }

            return newString.ToString();
        }

        public static bool EqualsCaseInsensitive(this string target, string other)
        {
            if (string.IsNullOrEmpty(target) && string.IsNullOrEmpty(other))
            {
                return true;
            }

            if (target == null && other != null)
            {
                return false;
            }

            return string.Compare(target, other, true) == 0;
        }

        public static bool ContainedAsFragment(this List<string> target, string input)
        {
            bool matchFound = false;
            input = input.ToLower();
            for (int i = 0; i < target.Count && !matchFound; i++)
            {
                matchFound = input.Contains(target[i].ToLower());
            }
            return matchFound;
        }

        public enum TrimToMaxOptions
        {
            None = 0,
            NoElipsis = 1
        }

        public static string TrimToMax(this string target, int maxLength = 80, TrimToMaxOptions options = TrimToMaxOptions.None)
        {
            if (target == null)
            {
                return null;
            }
            var elipsis = "...";
            if (options == TrimToMaxOptions.NoElipsis)
            {
                elipsis = string.Empty;
            }
            if (target.Length > maxLength)
            {
                return target.Substring(0, maxLength - elipsis.Length) + elipsis;
            }
            return target;
        }

        /// <summary>
        /// http://weblogs.asp.net/jgalloway/archive/2005/09/27/426087.aspx
        /// </summary>
        public static List<string> SplitCamelCase(this string source)
        {
            if (source == null)
                return new List<string> { }; //Return empty array.

            if (source.Length == 0)
                return new List<string> { "" };

            var words = new List<string>();
            int wordStartIndex = 0;

            char[] letters = source.ToCharArray();
            // Skip the first letter. we don't care what case it is.
            for (int i = 1; i < letters.Length; i++)
            {
                if (char.IsUpper(letters[i]))
                {
                    //Grab everything before the current index.
                    words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
                    wordStartIndex = i;
                }
            }

            //We need to have the last word.
            words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

            //Copy to a string array.
            string[] wordArray = new string[words.Count];
            words.CopyTo(wordArray, 0);

            List<string> stringList = new List<string>();
            stringList.AddRange(wordArray);
            return stringList;
        }
    }
}
