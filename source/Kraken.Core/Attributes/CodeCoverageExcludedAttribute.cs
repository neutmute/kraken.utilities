using System;

namespace Kraken.Core
{
    /// <summary>
    /// Tag methods with this attribute if you want to exclude them
    /// from NCover code coverage analysis.
    /// </summary>
    /// <remarks>
    /// A prime candidate for such exclusion is the private constructor 
    /// on a static class, which would never normally be called.
    /// </remarks>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class CodeCoverageExcludedAttribute : Attribute
    {
        /// <summary>
        /// Mark an area as excepted from coverage testing
        /// </summary>
        public CodeCoverageExcludedAttribute()
        { }

        /// <summary>
        /// Mark an area as excepted from coverage testing
        /// </summary>
        /// <remarks>Allow an reason to be provided. The reason is not used, but is handy for creating an area to comment why the code is being marked excluded</remarks>
        public CodeCoverageExcludedAttribute(string reason)
        { }
    }
}
