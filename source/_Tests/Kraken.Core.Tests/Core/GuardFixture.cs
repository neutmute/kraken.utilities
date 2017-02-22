using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kraken.Core.Tests.TestClasses;
using NUnit.Framework;

namespace Kraken.Core.Tests.Core
{
    public class GuardFixture : Fixture
    {
        [Test]
        public void EnumZero()
        {
            Assert.Throws<KrakenException>( ()=> Guard.EnumIsZero(Colour.Unknown));
        }
        
        [Test]
        public void That()
        {
            var value = -1;
            Assert.Throws<KrakenException>(() => Guard.That(value > 0, "WTF?"));
        }
        
        [Test]
        public void Against()
        {
            var uninitialised = int.MinValue;
            Assert.Throws<KrakenException>(() => Guard.Against(uninitialised == int.MinValue, "WTF?"));
        }

        [Test]
        public void EnumOk()
        {
            Guard.EnumIsZero(Colour.Red);
            // shouldn't throw
        }


        [Test]
        public void NullOrEmpty()
        {
            Assert.Throws<KrakenException>(() => Guard.NullOrEmpty("", "taskInfo.InputParam is required"));
        }

    }
}
