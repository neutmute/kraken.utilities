using System.Collections.Generic;

namespace Kraken.Tests
{
    /// <summary>
    /// Extensions methods for <see cref="List{String}"/>
    /// </summary>
    public static class ListStringExtensions
    {
        /// <summary>
        /// Syntactical sugar for adding several values at once
        /// </summary>
        public static void Add(this List<string> target, params string[] values)
        {
            target.AddRange(values);
        }
    }
}
