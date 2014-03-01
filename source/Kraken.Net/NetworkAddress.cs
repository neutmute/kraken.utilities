using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kraken.Net
{
    /// <summary>
    /// Wrapper for IP Address that makes persistence easier
    /// </summary>
    [Serializable]
    public class NetworkAddress : IComparable, IComparable<NetworkAddress>
    {
        #region Properties

        /// <summary>
        /// The actual backing store and used for persistance
        /// </summary>
        public byte[] Bytes { get; set; }

        // For OO use
        public IPAddress Address
        {
            get { return new IPAddress(Bytes); }
        }
        #endregion

        #region Ctor
        public NetworkAddress(byte[] bytes)
        {
            Bytes = bytes;
        }

        public NetworkAddress(long newAddress)
        {
            IPAddress address = new IPAddress(newAddress);
            Bytes = address.GetAddressBytes();
        }

        public NetworkAddress(IPAddress ipAddress)
        {
            Bytes = ipAddress.GetAddressBytes();
        }

        public NetworkAddress() : this(0)
        {
        }

        public NetworkAddress(string ipAddress)
        {
            Bytes = IPAddress.Parse(ipAddress).GetAddressBytes();
        }
        #endregion

        #region Methods


        public override string ToString()
        {
            return Address.ToString();
        }

        public override bool Equals(object obj)
        {
            var otherAsNetworkAddress = obj as NetworkAddress;
            var result = otherAsNetworkAddress != null && otherAsNetworkAddress.GetHashCode() == GetHashCode();
            return result;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as NetworkAddress);
        }

        public int CompareTo(NetworkAddress other)
        {
            if (other == null)
            {
                return -1;
            }
            return GetComparableToNumber().CompareTo(other.GetComparableToNumber());
        }

        /// <summary>
        /// Reflector observered implementation of IPAddress.Address which is depcreated as it doesn't work with ipv6
        /// </summary>
        /// <returns></returns>
        private long GetComparableToNumber()
        {
            byte[] bytes = Address.GetAddressBytes();
            long number = ((((bytes[3] << 0x18) | (bytes[2] << 0x10)) | (bytes[1] << 8)) | bytes[0]) & (0xffffffffL);
            return number;
        }

        #endregion

    }
}
