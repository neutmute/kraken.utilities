using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Kraken.Net
{
    public static class IPAddressExtensions
    {
        public static string ToString(this IPAddress address, bool humanReadable)
        {
            if (humanReadable)
            {
                byte[] bytes = address.GetAddressBytes();
                string ipString = null;
                for (int i = 0; i < bytes.Length - 1; i++)
                {
                    ipString += bytes[i] + ".";
                }
                return ipString + bytes[bytes.Length - 1];
            }


            return address.ToString();
        }
    }
}
