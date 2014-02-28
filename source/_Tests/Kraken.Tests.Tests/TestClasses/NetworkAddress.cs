using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Tests.Tests.TestClasses
{
    public class NetworkAddress
    {

       // public int Apple { get; set; }
        public Byte[] Bytes { get; set; }

        public NetworkAddress(Byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}
