using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Kraken.Net
{
    /// <summary>
    /// Wrapper to make it easy to pass around and serialise
    /// </summary>
    [Serializable]
    public class MacAddress
    {
        /// <summary>
        /// The actual backing store and used for persistance
        /// </summary>
        public byte[] Bytes { get; set; }

        public PhysicalAddress Address
        {
            get { return new PhysicalAddress(Bytes); }
        }


        #region Ctor
        public MacAddress(byte[] bytes)
        {
            Bytes = bytes;
        }

        public MacAddress(): this(new byte[0])
        {
        }
        #endregion

        #region Methods

        public override string ToString()
        {
            byte[] addressBytes = Address.GetAddressBytes();
            if (addressBytes.Length == 0 || addressBytes.Length == 1 && addressBytes[0] == 0)
            {
                return string.Empty;
            }
            return string.Join("-", (from z in addressBytes select z.ToString("X2")).ToArray());
        }
        
        public override bool Equals(object obj)
        {
            var otherAsMacAddress = obj as MacAddress;
            var result = otherAsMacAddress != null && otherAsMacAddress.GetHashCode() == GetHashCode();
            return result;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }
        #endregion
    }
}
