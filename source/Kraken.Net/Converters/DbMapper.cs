using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Kraken.Net;

namespace Kraken.Core
{
    public static class DbMapper
    {
        public static NetworkAddress ToNetworkAddress(IDataRecord row, string columnName)
        {
            byte[] bytes = row[columnName] as byte[];
            if (bytes == null)
            {
                return null;
            }
            return new NetworkAddress(bytes);
        }

        public static object FromNetworkAddress(NetworkAddress networkAddress)
        {
            if (networkAddress == null)
            {
                return null;
            }
            return networkAddress.Bytes;
        }

        public static MacAddress ToMacAddress(IDataRecord row, string columnName)
        {
            byte[] bytes = row[columnName] as byte[];
            if (bytes == null)
            {
                return null;
            }
            return new MacAddress(bytes);
        }

        public static object FromMacAddress(MacAddress macAddress)
        {
            if (macAddress == null)
            {
                return null;
            }
            return macAddress.Bytes;
        }
    }
}
