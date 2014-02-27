using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace Kraken.Net
{
    public static class PhysicalAddressExtensions
    {
        public static string ToString(this PhysicalAddress physicalAddress, bool hexFormat)
        {
            if (hexFormat)
            {
                StringBuilder sb = new StringBuilder();
                byte[] bytes = physicalAddress.GetAddressBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    sb.AppendFormat("{0}", bytes[i].ToString("X2"));
                    // Insert a hyphen after each byte, unless we are at the end of the 
                    // address.
                    if (i != bytes.Length - 1)
                    {
                        sb.Append("-");
                    }
                }
                return sb.ToString();
            }
            return physicalAddress.ToString();
        }
    }
}
