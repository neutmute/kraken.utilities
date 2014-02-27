using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Net;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core.Net
{
    public class NetworkAddressFixture : Fixture
    {
        [Test]
        public void Equality_Success()
        {
            NetworkAddress a1 = new NetworkAddress(1);
            NetworkAddress a2 = new NetworkAddress(2);
            NetworkAddress a3 = new NetworkAddress(2);

            Assert.IsFalse(a1.Equals(a2));
            Assert.IsTrue(a2.Equals(a3));
        }

        [Test]
        public void GetHashCode_Success()
        {
            NetworkAddress a1 = new NetworkAddress(1);
            NetworkAddress a2 = new NetworkAddress(2);
            NetworkAddress a3 = new NetworkAddress(2);

            int a2Hash = a2.GetHashCode();
            int a3Hash = a2.GetHashCode();

            Assert.IsFalse(a1.GetHashCode() == a2.GetHashCode());
            Assert.AreEqual(a2Hash, a3Hash);
        }
    }
}
