using System;
using System.Collections.Generic;

namespace Kraken.Framework.TestMonkey
{
    /// <summary>
    /// Allows more complex options to be passed into the code generator
    /// </summary>
    public class CodeGenOptions : ObjectInspectorOptions
    {
        #region Fields
        /// <summary>
        /// Use this in conjunction with EnumerateAllProperties = false
        /// </summary>
        private List<Type> _UpcastTypes;
        #endregion

        #region Properties
        /// <summary>
        /// List of types that should be automatically cast to their actual types (not necesairily the type exposed by the target)
        /// </summary>
        /// <remarks>
        /// Actual type since we must have a reference to the type we want to upcast
        /// </remarks>
        public List<Type> UpcastTypes
        {
            get { return _UpcastTypes; }
        }
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="CodeGenOptions"/>.
        /// </summary>
        public CodeGenOptions()
        {
            _UpcastTypes = new List<Type>();
            BindStatic = true;
        }
    }

   
}
