///*
// * Use DisplayNameAttribute 
// */

using System;

namespace Kraken.Core
{
    /// <summary>
    /// ComponentModel DisplayName won't map onto Enums
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : Attribute
    {
        public string Name { get; private set; }

        /// <summary>
        /// Used to mark what an enum casts to as its short varchar Name
        /// </summary>
        public EnumDisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
}
