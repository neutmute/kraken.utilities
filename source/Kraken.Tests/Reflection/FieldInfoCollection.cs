using System.Collections.Generic;
using System.Reflection;

namespace Kraken.Framework.TestMonkey
{
    /// <summary>
    /// A <see cref="List{T}"/> of <see cref="MemberInfo"/> instances that implements a <see cref="Contains"/> method.
    /// </summary>
    public class FieldInfoCollection : List<MemberInfo>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="FieldInfoCollection"/>.
        /// </summary>
        public FieldInfoCollection()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="FieldInfoCollection"/> from the supplied <paramref name="seed"/>.
        /// </summary>
        public FieldInfoCollection(IEnumerable<MemberInfo> seed)
        {
            AddRange(seed);
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// FieldInfo doesnt implement gethashcode, so implement our own contains
        /// </summary>
        public bool Contains(FieldInfo fieldInfo)
        {
            return Exists(f => f.Name == fieldInfo.Name);
        }
        #endregion
    }

}
